# Plant Game

This project is a small 3D Unity prototype where the player fights waves of enemies using a water-based weapon and movement system. The objective is to protect the main plant while simultaneously collecting water from the environment in order to move, shoot, and water the plant.

## Features

- Movement System
- Weapon System
- Enemy-Spawn System
- Enemy-Behaviour
- Resource-Drop System
- Plant State System
- Water-System for Plant, Weapons and Movement

- Roguelite Skill-Tree System
- Roguelite Resource System

## Documentation
[Design Document in German](https://github.com/Jutsn/Plant-Game-Macromedia/blob/main/Documentation/GDD%20Terraplantation.pdf)

## Git Strategy 

- **Branching Model**
  - Master Branch: Holds the stable, production-ready version of the project.
  - Feature Branches: Used for developing new features, bug fixes, or experiments. Each developer works in their own branch.
  - Pull Requests: Changes are merged into the main branch via pull requests, ideally after code review and testing.

- **Commit Best Practices**
  - Commit early and often: Frequent commits help track progress and make it easier to revert changes if needed.
  - Clear commit messages: Use descriptive messages like "Add biofeedback interface" or "Fix prefab loading issue".

- **Process**
  1. Create a new branch off master in which to do feature or bug work.
  2. Commit often to this branch and push those changes to back them up.
  3. When ready, try to merge the feature branch to the main branch.
  4. When merge errors occure, contact me for help or create a pull request. Don't force push etc.!
  5. When no errors occure and everything in Unity works as expected, commit and push the merge.
