# Related work

Literature check of 2026-10-24 for the paper on clinical agents that misread a hidden evidence state.
Only abstracts and overview pages were read for the three closest papers; read the full texts before
writing the related-work section.

## Positioning

The bare phenomenon — an LLM agent treats an empty or silently failed tool result as real data — is
already documented (2024–2026). The paper must not claim it as new.

The open gap this work fills:

1. **Controlled visible-vs-hidden evidence state with a clinical safety outcome.** Same tool, only the
   loss of state differs (conditions B vs C1); the outcome is unsafe prescribing.
2. **The cause of `unknown` is an authorization / governance denial** (`access_denied`: consent, physician
   rights), not a generic tool fault.
3. **Detection without trusting the agent:** an independent audit against the governance journal
   (MFSSIA + DKG), not an LLM judge or the agent's own report.
4. **Declaration vs journal (C2, C3):** whether a substitution the agent cannot see is caught by
   checking its declaration against the journal.

Working thesis: *loss of evidence state between the governance layer and a clinical agent makes
prescribing unsafe, and the governance journal makes it detectable.*

## Closest work

| Work | Finding | Difference from this work |
|---|---|---|
| **Guardrails as Scapegoats** — A. Singh, arXiv 2607.19449, KDD workshop 2026 | Silent failures injected as HTTP 200 with empty / null / malformed / truncated payloads; 56.6% of valid responses fabricate data; 4 models, 12 tools incl. `fetch_medical_record` (synthetic) | Closest in mechanism. No visible-vs-hidden condition, no clinical decision outcome, LLM-judge classification, no governance journal |
| **Silent Failures in Agent–Tool Interaction: An Audit of ToolUniverse** — Singh, Gopalan, Narayanan, arXiv 2609.26836, NeurIPS 2026 GenAI4Health and AgenticLS workshops | Biomedical tools: APIs and wrappers silently lose completeness (missing fields, caps, filters). Agents disclose loud failures (timeouts, authorization errors) but not silent ones | Closest in domain. Failures audited, not injected; downstream decisions not measured; recommends tool-side disclosure, no external journal |
| **When Absence Is Evidence (CROWN-QA)** — Min et al., arXiv 2608.04591, NeurIPS 2026 | Models fail to separate a certified negative from *unknown*; they over-close, treating partial evidence as complete | Exactly the `confirmed_absent` vs `unknown` distinction, but document QA — no agents, no clinical setting |

## Tool failures in agents

- **Tools Fail: Detecting Silent Errors in Faulty Tools** — CMU and Cohere, 2024, arXiv 2406.19228. Most
  tool failures are silent; a model must detect a failure before it can recover.
- **Beyond Function Calling: Tool-Environment Unreliability** — arXiv 2606.25819. No model above 0.60
  overall; robust execution under unreliable tools has to be trained for.
- **ToolMaze** — 2026. Explicit/implicit × transient/permanent perturbations; implicit semantic failures
  hurt most.
- **ToolFailBench** — ICML 2026, arXiv 2607.04686. Failure taxonomy: tool-skipping, result-ignoring,
  output fabrication, unnecessary tool use.

## Clinical reasoning under incomplete evidence

- **ClinDet-Bench** — arXiv 2602.22771. Determinable vs undeterminable cases; models judge prematurely and
  over-abstain, partly by assuming plausible values for missing inputs.
- **Decide, Ask, or Defer: Clinical LLMs under Incomplete Evidence** — NeurIPS 2026. Separates asking for
  information from deferring to a clinician; safety–autonomy trade-off.
- **KnowGuard** — ICLR 2026, arXiv 2509.24816. Investigate-before-abstain over a medical knowledge graph.
- **Knowing When to Abstain: Medical LLMs Under Clinical Uncertainty** — Machcha et al., EACL 2026.
  Models fail to abstain when uncertain; explicit abstention options help more than scale.

In all of these the incompleteness is given in the task text; here it arises from infrastructure.

## Medical agents with EHR / FHIR tools

- **MedAgentBench** — arXiv 2501.14654. Virtual EHR environment; most common error is not following the
  instruction format.
- **FHIR-AgentBench** — arXiv 2509.19319. Agents fail to pick the right FHIR resource type; best ~50%.

Neither covers access denial, consent or authorization.

## Auditing agents against execution traces

- **LEDGER: Claim-to-Evidence Trace Graphs for Auditing LLM Agents** — arXiv 2608.18398.
- **AgentAudit** — arXiv 2609.09875. Reads only the recorded execution trace.
- **RealClawBench** — arXiv 2606.03889. Execution evidence cut false "done" verdicts from 21.0% to 4.2%.

General-purpose; not healthcare and not a governance journal.

## Practitioner sources (not peer-reviewed)

- *No Results Is Not Absence: Why Agents Treat Retrieval Failure as Proof* — tianpan.co, 2026-04-23.
  Same intuition as this paper's thesis; frames it as a tool-contract design bug.
- *Tool-use API design for LLMs: 5 patterns that prevent agent loops and silent failures* — dev.to.
  Recommends explicit status fields so "no results" differs from "failed".

## Sources

- https://arxiv.org/html/2607.19449v1
- https://arxiv.org/html/2609.26836
- https://www.alphaxiv.org/abs/2608.04591
- https://arxiv.org/html/2406.19228v1
- https://arxiv.org/pdf/2606.25819
- https://www.emergentmind.com/papers/2606.05806
- https://www.alphaxiv.org/abs/2607.04686
- https://arxiv.org/html/2602.22771v1
- https://neurips.cc/virtual/2026/177173
- https://arxiv.org/pdf/2509.24816
- https://preview.aclanthology.org/ingest-eacl/2026.eacl-long.291
- https://arxiv.org/pdf/2501.14654
- https://arxiv.org/pdf/2509.19319
- https://arxiv.org/pdf/2608.18398
- https://www.alphaxiv.org/abs/2609.09875.md
- https://arxiv.org/pdf/2606.03889
- https://tianpan.co/blog/2026-04-23-no-results-is-not-absence-agent-retrieval-negation
- https://dev.to/adamo_software/tool-use-api-design-for-llms-5-patterns-that-prevent-agent-loops-and-silent-failures-f29
