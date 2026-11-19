# Evidence-boundary prescription agent experiment (C#)

C# / .NET 10. Conditions A, B, C1, C2, C3. Inputs: `data/` (hand labels, policy, custodians,
scenarios) and `prompts/`. It talks to MFSSIA only over HTTP (`GET /evidence-status/:patientId`,
`GET /audit-events?runId=`).

## Layout

```
src/PrescriptionAgent/
  Program.cs       entry point: builds the host (configuration + DI) and runs Runner
  ServiceCollectionExtensions.cs  AddPrescriptionAgent(): every registration, the JSON serializer settings; used by Program and tests
  Services/        one folder per part (namespace PrescriptionAgent.Services.<Folder>):
    Experiment/    Runner (CLI, run plan), RunArguments
    ExperimentSetup/  ExperimentPaths, Prompts, YamlLoader, ExperimentDataLoader (data/, prompts/); Preflight, OfflineGateCheck
    Agents/        Agent (agent loop), Toolbox (tools), EvidenceSubstitution (lost/full boundary), ToolArguments, DecisionValidator
    Gate/          PolicyGate: condition A
    Mfssia/        HTTP client (unwraps MFSSIA's {success, data} envelope)
  Evaluation/      policy logic shared by the gate (A) and audit check 3
  Auditing/        independent audit against the MFSSIA journal
  Configuration/   PrescriptionAgentOptions and how they are read (PrescriptionAgentOptionsSetup)
  Adapters/        model interface and IModelAdapterFactory, Anthropic (official SDK), OpenAI-compatible (raw HTTP)
  Models/          one type per file, namespace = folder (PrescriptionAgent.Models.<Folder>):
    Mfssia/        what MFSSIA returns: EvidenceResponse, EvidenceSource, AuditEvent, MfssiaEnvelope
    Scenario/      data/ scenarios, custodians, tasks: ExperimentData, Patient, ExpectedEvidence, Custodian, ClinicalTask
    Policy/        prescribing rules and their evaluation: PolicyFile, DrugRule, LabThreshold, Observation, Verdict, ...
    Tools/         agent tools: ToolDefinition, ObjectSchema, tool arguments and answers
    Llm/           the model-adapter contract: ToolSpec, ToolCall, Turn
    Decisions/     the final decision: Decision, DeclarationItem
    Runs/          a run and its result: RunKey (runId), Condition, ToolCallRecord, AgentResult, GateResult, RunOutcome, AuditCheck
    OpenAI/        /chat/completions wire types
  Exceptions/      MfssiaException, DecisionValidationException, ModelApiException, ModelQuotaExceededException, RunnerExitException
  Reporting/       Score, ResultRow (+ ResultRow.From), ResultsCsv, SummaryReport, GateWarning, Transcript
tests/PrescriptionAgent.Tests/ xUnit tests; no API calls, no live MFSSIA.
                   TestHost builds the same container with configuration in memory and fakes swapped in.
out/               results.csv, summary.md, transcripts/{runId}.jsonl
```

## Running

```
dotnet test

# MFSSIA: PILOT_DEMO_SEED=true EVIDENCE_REFERENCE_TIME=2026-10-10T00:00:00Z
export MFSSIA_URL=http://localhost:4000/api ANTHROPIC_API_KEY=... AGENT_MODELS=claude-opus-5-5,claude-sonnet-5-5
dotnet run --project src/PrescriptionAgent -- --smoke       # 1 model, 1 repetition: 8 gate + 32 agent runs
dotnet run --project src/PrescriptionAgent --               # full matrix (AGENT_REPS, default 10)
dotnet run --project src/PrescriptionAgent -- report        # rebuild summary.md from results.csv

# OpenAI-compatible APIs: prefix a model with oai:
AGENT_OPENAI_BASE_URL=https://openrouter.ai/api/v1 AGENT_OPENAI_API_KEY=... \
  AGENT_MODELS=oai:nvidia/nemotron-3-ultra-550b-a55b:free dotnet run --project src/PrescriptionAgent -- --smoke
AGENT_OPENAI_BASE_URL=http://localhost:11434/v1 AGENT_MODELS=oai:qwen3:14b dotnet run --project src/PrescriptionAgent -- --smoke
```

Options: `--conditions`, `--tasks`, `--patients`, `--models`, `--reps`, `--tag`
(suffix for runIds that already exist in the MFSSIA journal), `--out`.

### Configuration

Settings are `PrescriptionAgentOptions`, read from the `PrescriptionAgent` section of
[src/PrescriptionAgent/appsettings.json](src/PrescriptionAgent/appsettings.json)
(copied next to the built program, so it is found from any working directory) and from the environment
variables, which win: `MFSSIA_URL`, `AGENT_MODELS`,
`AGENT_REPS`, `AGENT_TEMPERATURE` (default 0, `none` to omit), `AGENT_EFFORT`, `AGENT_OPENAI_BASE_URL`,
`AGENT_OPENAI_API_KEY`, `AGENT_DIR` (path to this repository if it is not found by walking up).
`ANTHROPIC_API_KEY` is read by the Anthropic SDK. `--models` and `--reps` override the settings for one run.

```json
{ "PrescriptionAgent": { "Mfssia": { "Url": "http://localhost:4000/api" }, "Run": { "Models": ["claude-opus-5-5"], "Reps": 5 } } }
```

Keys (`AGENT_OPENAI_API_KEY`, `ANTHROPIC_API_KEY`) belong in the environment, not in appsettings.json.

## Notes

- **Temperature with Anthropic models.** The C# SDK (13.x) has no sampling parameters, so temperature is
  never sent and `results.csv` says `not supported by the SDK; not applied`. OpenAI-compatible APIs get it.
- **Tool schemas sent to Anthropic** include `additionalProperties: false`, as written in `tools.yaml`.
- **Data are classes, serialised by System.Text.Json** (one `JsonSerializerOptions`, registered in `AddPrescriptionAgent`:
  snake_case for the harness's own records, MFSSIA's camelCase names given explicitly, compact output,
  non-ASCII kept as is). JSON stays free-form (`JsonElement`) only where it is: the evidence `value`
  (a number or a list), the agent's `value_used`, tool schemas and raw provider responses. An MFSSIA answer
  is shown to the model with MFSSIA's field order and without the fields MFSSIA left out; unknown fields are kept.
- **A malformed `submit_decision`** is rejected with System.Text.Json's message (missing required
  property, wrong type) or the harness's own rule (decision value, observed value, drug when prescribing).
