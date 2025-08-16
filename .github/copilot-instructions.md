# Drakken Laser Drill (Continued) - GitHub Copilot Instructions

## Mod Overview and Purpose
The Drakken Laser Drill mod introduces a powerful and infinitely upgradeable laser turret into RimWorld. Originally developed by 空曜, the mod has been updated to provide additional features and improved compatibility. The laser turret, called the Drakken Laser Drill, operates with advanced research technology that allows for limitless enhancements in power storage, damage, and armor penetration. The mod enhances your defensive capabilities with strategic options and unique functionalities.

## Key Features and Systems
- **Infinite Range**: The Drakken Laser Drill has an unbounded attack range, allowing strategic placements.
- **Dynamic Power Based Damage**: The turret's damage output is directly linked to its power consumption.
- **Technology Driven Upgrades**: Research new technology to continuously upgrade turret power, damage, and armor penetration.
- **Active Skills**: Unlock two active skills, charging up through energy expended during attacks.
- **Immortality Protocol**: The turret enters a repair state instead of being destroyed upon reaching zero Hitpoints.
- **Configurable Settings**: Adjust various parameters through mod settings for personalized gameplay.

### Control by Icon
- **Attack via Frame Selection**: Conduct targeted attacks by selecting enemy zones with intuitive box controls.
- **Attack All Enemies**: Automate targeting of all enemies on the map, prioritizing pawns over wild animals.
- **Automatic Attack Mode**: Automatically engage enemies during attack incidents with a built-in delay for drop pods.
- **Mouse-guided Attack**: Increase power output for precise, mouse-guided strikes, mindful of potential friendly fire.
- **Concentrated Beam and Pulse Cannon Abilities**: Special high-power attacks targeting specific paths and areas.
  
## Coding Patterns and Conventions
The mod adheres to clear and consistent C# coding practices:
- **Class and Method Naming**: Follow PascalCase for classes and method names (e.g., `Building_DrakkenLaserDrill_Beacon_CrossMap`).
- **File Organization**: Group similar classes logically into files reflecting their functional role.
- **Modular Design**: Use extensive class and component architecture to separate concerns.

## XML Integration
- The XML files complement the C# codebase by defining game content, such as textures, item properties, and in-game behaviors.
- Use XML to manage data-driven aspects, providing easy-to-modify parameters for balance and customization.

## Harmony Patching
- **Harmony Utilization**: Implement Harmony patches to integrate seamlessly with the base game, customizing and extending game behaviors without overwriting existing code.
- **Patch Patterns**: Apply specific patches for game logic, focusing on automatic attacks, research protocols, and other turret interactions.

## Suggestions for GitHub Copilot
- **Functionality Suggestions**: Utilize Copilot for generating repetitive or boilerplate code such as property getters/setters, basic method structures, and XML configurations.
- **Pattern Recognition**: Get help with identifying correct Harmony patch points and implementing necessary patches effectively.
- **Documentation and Comments**: Benefit from code commenting, ensuring clear understanding of each method's purpose and logic.
- **Advanced Refactoring**: Employ Copilot to suggest refactoring opportunities, such as merging similar methods or classes for efficiency.

This document provides a comprehensive guide to understanding the structure, features, and coding standards of the Drakken Laser Drill mod. Utilize these instructions to maintain the mod, extend its functionality, and integrate seamlessly with RimWorld updates and community feedback.
