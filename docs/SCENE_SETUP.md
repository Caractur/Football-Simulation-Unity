# 🏟️ Scene Setup Guide - Football Simulation

Step-by-step instructions for building the first playable match scene from the scripts in this repository.

## 📋 **Step 1: Open the Project and Install Packages**

### 1.1 Open the Project
1. **Open Unity Hub** and choose **Add > Add project from disk**
2. **Select this repository's folder** and open it with Unity 6 (6000.0)

### 1.2 Install Packages
Open **Window > Package Manager**, click **`+` > Add package by name** and add:
- `com.unity.inputsystem` (Input System)
- `com.unity.ugui` (uGUI, which includes TextMeshPro in Unity 6)
- `com.unity.ai.navigation` (AI Navigation, for baking the NavMesh)

Then run **Window > TextMeshPro > Import TMP Essential Resources**.

### 1.3 Set Up Project Structure
Create these folders as you need them:
```
Assets/
├── Scripts/          (already in the repo)
├── Input/            (already in the repo)
├── Scenes/           (already in the repo)
├── Prefabs/
├── Materials/
├── Data/
└── UI/
```

## 🎮 **Step 2: Configure Input System**

### 2.1 Enable Input System
1. **Go to Edit > Project Settings > Player**
2. **Find "Active Input Handling"**
3. **Set it to "Both" or "Input System Package (New)"**
4. **Click "Yes" when prompted to restart**

### 2.2 Set Up Input Actions
1. **Select the `PlayerInputActions.inputactions` file**
2. **In Inspector, click "Generate C# Class"**
3. **Make sure "Generate C# Class" is checked**
4. **Click "Apply"**

## 🏟️ **Step 3: Create Basic Scene**

### 3.1 Create Football Field
1. **Create a new scene: File > New Scene > 3D**
2. **Save it as "GameScene" in Assets/Scenes/**

#### Create Field Geometry:
```
1. Create Plane (GameObject > 3D Object > Plane)
   - Scale: (2, 1, 1.4) for standard football field proportions
   - Position: (0, 0, 0)
   - Name: "FootballField"

2. Create Field Markings:
   - Create empty GameObject named "FieldMarkings"
   - Add child objects for:
     - Center circle (cylinder scaled to 0.1, 0.01, 0.1)
     - Center line (cube scaled to 0.01, 0.01, 1.4)
     - Penalty areas (cubes)
     - Goal areas (cubes)
```

### 3.2 Add Goals
```
1. Create Goal Posts:
   - Create 2 Cubes for each goal (left and right posts)
   - Scale: (0.1, 2, 0.1)
   - Position: At each end of the field
   - Name: "GoalPost_Left", "GoalPost_Right"

2. Create Crossbar:
   - Create 1 Cube for each goal
   - Scale: (0.1, 0.1, 0.7)
   - Position: Above the posts
   - Name: "Crossbar"
```

### 3.3 Add Lighting
```
1. Create Directional Light:
   - GameObject > Light > Directional Light
   - Position: (0, 10, 0)
   - Rotation: (45, 0, 0)
   - Intensity: 1.5

2. Create Ambient Light:
   - Window > Rendering > Lighting Settings
   - Set Ambient Mode to "Color"
   - Set Ambient Color to light blue
```

## ⚽ **Step 4: Create Game Objects**

### 4.1 Create Ball
```
1. Create Sphere:
   - GameObject > 3D Object > Sphere
   - Scale: (0.2, 0.2, 0.2)
   - Position: (0, 0.2, 0)
   - Name: "Football"

2. Add Components:
   - Rigidbody (Add Component > Physics > Rigidbody)
   - BallController script
   - Sphere Collider (should be there by default)

3. Configure Rigidbody:
   - Mass: 0.5
   - Drag: 1
   - Angular Drag: 0.5
   - Use Gravity: true
   - Is Kinematic: false
```

### 4.2 Create Player Prefab
```
1. Create Player GameObject:
   - GameObject > 3D Object > Capsule
   - Scale: (0.5, 1, 0.5)
   - Name: "Player"

2. Add Components:
   - Rigidbody
   - Capsule Collider
   - PlayerProfile script
   - PlayerInputHandler script (for protagonist)
   - AIController script (for AI players)
   - NavMeshAgent (for AI navigation)

3. Configure Components:
   - Rigidbody: Mass = 70, Drag = 5, Angular Drag = 5
   - NavMeshAgent: Speed = 5, Angular Speed = 120, Stopping Distance = 0.5
```

### 4.3 Create Team Prefabs
```
1. Create Team A Players:
   - Duplicate Player prefab 11 times
   - Name them: "Player_A_1" to "Player_A_11"
   - Position them in formation (4-4-2 recommended)

2. Create Team B Players:
   - Duplicate Player prefab 11 times
   - Name them: "Player_B_1" to "Player_B_11"
   - Position them in formation on opposite side

3. Set Team Colors:
   - Create materials for each team
   - Assign to player meshes
```

## 🎯 **Step 5: Set Up Game Manager**

### 5.1 Create Game Manager
```
1. Create Empty GameObject:
   - GameObject > Create Empty
   - Name: "GameManager"
   - Position: (0, 0, 0)

2. Add Components:
   - GameManager script
   - Tag as "GameManager"
```

### 5.2 Configure Game Manager
```
1. In Inspector, assign:
   - League Data (create ScriptableObject)
   - Player Team
   - Ball reference
   - UI Manager reference
   - Match settings
```

## 🎨 **Step 6: Create UI**

### 6.1 Create Canvas
```
1. Create UI Canvas:
   - GameObject > UI > Canvas
   - Name: "GameCanvas"
   - Set Render Mode to "Screen Space - Overlay"

2. Add Canvas Scaler:
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920 x 1080
   - Screen Match Mode: Match Width or Height
```

### 6.2 Create UI Elements
```
1. Create Score Display:
   - Right-click Canvas > UI > Text - TextMeshPro
   - Name: "ScoreText"
   - Position: Top center
   - Text: "0 - 0"

2. Create Match Time:
   - Right-click Canvas > UI > Text - TextMeshPro
   - Name: "TimeText"
   - Position: Top right
   - Text: "00:00"

3. Create Player Stats:
   - Right-click Canvas > UI > Panel
   - Name: "PlayerStatsPanel"
   - Position: Bottom left
   - Add child TextMeshPro elements for stats

4. Create League Progress:
   - Right-click Canvas > UI > Panel
   - Name: "LeagueProgressPanel"
   - Position: Top left
   - Add progress bar and text
```

### 6.3 Add UI Manager
```
1. Create Empty GameObject:
   - Name: "UIManager"
   - Add UIManager script

2. Assign UI references in Inspector:
   - Score Text
   - Time Text
   - Player Stats Panel
   - League Progress Panel
```

## 🤖 **Step 7: Set Up AI Navigation**

### 7.1 Bake NavMesh
```
1. Select the FootballField object
2. Add Component > NavMesh Surface (from the AI Navigation package)
3. Leave Agent Type on "Humanoid" and Collect Objects on "All Game Objects"
4. Click "Bake" on the NavMesh Surface component
5. Check that a blue NavMesh overlay appears on the pitch
```

### 7.2 Configure AI Agents
```
1. Select all AI players
2. In NavMeshAgent component:
   - Speed: 5
   - Angular Speed: 120
   - Stopping Distance: 0.5
   - Radius: 0.5
   - Height: 2
```

## 📊 **Step 8: Create Data Assets**

### 8.1 Create Team Data
```
1. Right-click in Project window > Create > Football > Team Data
2. Name: "TeamA_Data"
3. Configure:
   - Team Name: "Team A"
   - Kit Colors
   - Difficulty: 1
   - Add 11 Player Data entries
4. Repeat for Team B, C, D, E with increasing difficulty
```

### 8.2 Create League Data
```
1. Right-click in Project window > Create > Football > League Data
2. Name: "League_Data"
3. Configure:
   - League Name: "Football League"
   - Total Weeks: 5
   - Add all Team Data assets
```

### 8.3 Assign Data to Game Manager
```
1. Select GameManager
2. In Inspector, assign:
   - League Data: League_Data
   - Player Team: TeamA_Data
```

## 🎮 **Step 9: Test Basic Functionality**

### 9.1 Test Player Movement
```
1. Enter Play Mode
2. Use WASD to move protagonist
3. Verify ball physics
4. Check AI movement
```

### 9.2 Test Ball Interaction
```
1. Move protagonist near ball
2. Press Space to kick
3. Press E to pass
4. Verify possession transfer
```

### 9.3 Test AI Behavior
```
1. Let AI players move
2. Check formation positioning
3. Verify ball pursuit
4. Test passing between AI
```

## 🔧 **Step 10: Polish and Refine**

### 10.1 Add Materials and Textures
```
1. Create field grass material
2. Create player kit materials
3. Create ball texture
4. Apply to respective objects
```

### 10.2 Add Basic Animations
```
1. Create Animator Controllers for players
2. Add basic idle, run, kick animations
3. Assign to PlayerProfile components
```

### 10.3 Add Sound Effects
```
1. Import football sound effects
2. Add AudioSource components
3. Trigger sounds on events (kick, pass, goal)
```

## 🚀 **Step 11: Final Testing**

### 11.1 Full Game Test
```
1. Start a complete match
2. Test all gameplay mechanics
3. Verify UI updates
4. Check league progression
```

### 11.2 Performance Check
```
1. Monitor frame rate
2. Check for memory leaks
3. Optimize if needed
```

## 📝 **Troubleshooting Common Issues**

### Issue: Input System Not Working
- **Solution**: Ensure Input System package is installed and Active Input Handling is set correctly

### Issue: NavMesh Not Working
- **Solution**: Make sure the pitch has a NavMesh Surface component and that you've clicked "Bake" on it

### Issue: Scripts Not Found
- **Solution**: Check that all scripts are in the correct folders and have no compilation errors

### Issue: Ball Physics Issues
- **Solution**: Adjust Rigidbody settings and check for proper collision detection

## 🎯 **Next Development Priorities**

1. **Player Switching System** - Implement during stoppages
2. **Advanced AI Tactics** - Improve decision making
3. **Multiple Formations** - Add different team formations
4. **Player Animations** - Add more realistic animations
5. **Sound and Music** - Add audio feedback
6. **Mobile Support** - Adapt for mobile platforms
7. **Multiplayer** - Add online multiplayer
8. **Career Mode** - Add player development system

## 📞 **Need Help?**

If you encounter any issues during setup:
1. Check the Unity Console for error messages
2. Verify all scripts are properly assigned
3. Ensure all required components are added
4. Check the README.md for additional details

Good luck with your football simulation game! ⚽🎮
