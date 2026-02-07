# Crimson Compass — Audio Architecture (Mermaid)

    ```mermaid
    flowchart TD
      A[Gameplay / Narrative Scripts
Manual Event Timing] -->|Play UI/PRESS/SFX/ECHO| B[CCAudioManual API
(single call surface)]
      B -->|guarded play requests| C[CCCanonAudioGuardrails
- 1 ECHO / episode
- 1 Disproof / episode
- warrant axis rules]
      C -->|allowed events| D[CCAudioDirector.Play(eventId)]

      E[CCAudioContextProvider
episodeNumber + state bands] --> F[CCAudioDeltaApplier
episode → delta]
      F --> G[CCEpisodeAudioDeltaSO
bus/event/stateBand deltas]

      E --> H[State Bands
HEAT / TIME / LEAD_INTEGRITY]
      H --> G

      G -->|Additive stack| D
      D -->|bus routing| I[Unity AudioMixer
AMBIENCE/SFX/UI/VO/PRESS/ECHO]
      D -->|audio clips| J[AudioSources
routed to bus groups]
      I --> K[Master Output]
      J --> K

      L[Debug Tools
Episode cycler + QA window] --> E
      L --> F
    ```

    ```mermaid
    sequenceDiagram
      participant Game as Game Script (Manual Timing)
      participant Manual as CCAudioManual (API)
      participant Guard as Canon Guardrails
      participant Ctx as Audio Context Provider
      participant Lib as Delta Library
      participant Delta as Episode Delta SO
      participant Dir as Audio Director
      participant Mix as Unity AudioMixer

      Game->>Manual: UI("confirm_01")
      Manual->>Guard: Is this allowed? (rules)
      Guard-->>Manual: Allowed
      Manual->>Dir: Play(eventId, bus=UI)

      Ctx->>Lib: episodeNumber lookup
      Lib-->>Delta: CCEpisodeAudioDeltaSO for episode
      Dir->>Delta: Get bus/event/stateBand offsets
      Dir->>Dir: totalDb = variant + bus + event + stateBand
      Dir->>Dir: pitchCents = variant + eventPitchDelta
      Dir->>Mix: Apply bus dB to exposed params
      Dir-->>Game: sound plays with correct meaning
    ```
