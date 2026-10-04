# Football Simulation (Unity)

A single-player football game in Unity 6. You control one player in an 11-a-side match while AI handles your teammates and the opposition, and you work your way up a five-team league in which every opponent is harder than the last.

> **Status: early prototype.** The gameplay scripts are written, but the playable scene hasn't been built yet. The project ships with an empty `MainMenu` scene and no prefabs or team data, and it won't compile until you install the packages listed below. [`docs/SCENE_SETUP.md`](docs/SCENE_SETUP.md) walks through building the first match scene.

## What's in the code

```
Assets/
├── Input/PlayerInputActions.inputactions   Move, Sprint, Kick, Pass, SwitchPlayer
└── Scripts/
    ├── Data/
    │   ├── PlayerStats.cs      Shooting, defending, pace and passing (1–100)
    │   ├── Position.cs         Position enum
    │   ├── PlayerProfile.cs    Per-player component: stats, team, possession state
    │   ├── Team.cs             Roster, colours and overall rating
    │   ├── TeamData.cs         ScriptableObject: team, players and formation
    │   ├── League.cs           Opponent order, week counter, progress
    │   └── LeagueData.cs       ScriptableObject: the five teams, validated by difficulty
    ├── Managers/GameManager.cs Singleton that runs matches, score, timer and league progress
    ├── Input/PlayerInputHandler.cs  Drives the protagonist from the Input System actions
    ├── AI/AIController.cs      NavMesh-based teammates and opponents
    ├── Gameplay/BallController.cs   Ball physics, kicks, passes and possession transfer
    └── UI/UIManager.cs         Score, timer, protagonist stats and result screens
```

The AI re-evaluates the play on a timer instead of every frame. A player with the ball shoots once inside shooting range, otherwise it passes or dribbles towards goal. Without the ball, the closest player within vision range chases it (anyone nearby goes for a loose ball), the others move into support positions, and players who can't see the ball drift back to a formation spot. The ball hands possession to whichever player is closest within a small radius, with a short cooldown so it doesn't flicker between players. `GameManager` raises `UnityEvent`s for score changes, match start and end, and league completion so the UI can listen without hard references.

Matches currently last 90 real-time seconds (`GameManager.matchDuration`).

## Controls

| Action | Keys |
|---|---|
| Move | WASD or arrow keys |
| Sprint | Left Shift |
| Kick / shoot | Space |
| Pass | E |
| Switch player | Q (bound, not implemented yet) |

## Getting it running

1. Open the folder with **Unity 6 (6000.0.55f1** or a later 6000.0 release).
2. In **Window > Package Manager**, choose **`+` > Add package by name** and add:
   - `com.unity.inputsystem`
   - `com.unity.ugui` (this includes TextMeshPro in Unity 6)
   - `com.unity.ai.navigation`
3. Run **Window > TextMeshPro > Import TMP Essential Resources**.
4. Under **Edit > Project Settings > Player**, set **Active Input Handling** to *Input System Package (New)* or *Both* and let the editor restart.

The scripts should compile at that point. Then follow [`docs/SCENE_SETUP.md`](docs/SCENE_SETUP.md) to build the pitch, ball, player prefabs, UI, NavMesh and the five `TeamData` assets plus the `LeagueData` asset (both available under **Assets > Create > Football**).

## Still to do

- Build the match scene, prefabs and data assets
- Player switching on **Q** during stoppages
- Formations beyond the single simplified layout `AIController` uses now
- Animations, audio and ball-physics tuning
