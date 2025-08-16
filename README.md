# [Drakken Laser Drill (Continued)](https://steamcommunity.com/sharedfiles/filedetails/?id=3550292796)

![Image](https://i.imgur.com/buuPQel.png)

Update of 空曜s mod https://steamcommunity.com/sharedfiles/filedetails/?id=2941974740

![Image](https://i.imgur.com/pufA0kM.png)
	
![Image](https://i.imgur.com/Z4GOv8H.png)

This Mod provides a giant laser turret that can be upgraded infinitely through research technology: Drakken Laser Drill (technology need: Electricity)


1.Characteristic：
A. Infinite range
B. Damage caused by power consumption (initial: 36/s) 【Mineable】
C. Unlimited upgrade of power storage, damage and armor penetration through research technology
D. Unlock 2 active skills through research and technology (charging of active skills: the power consumed during the attack will increase the charging of active skills by an equal amount)
E. Immortality Protocol: when the Hitpoint returns to 0, it will not be destroyed, but will enter the repair state, and cannot be attacked in the repair state. Restart the turret after Hitpoint fixed to Max
F. Some data can be adjusted in Mod settings


2. Control by icon
A. Attack · Frame selection: two clicks are required. The first click is to confirm the box to select the starting position, and the second click is to confirm the box to select the ending position. After confirmation, attack all "Buildings and Pawn" within the frame selection range (priority to attack Pawn)
B. Attack · All enemies: click to automatically search all "Enemies" on the current map (wild animals will not be attacked)
C. Auto: When on, the enemy will be attacked automatically when the attack incident (if the enemy arrives through the drop pod , there will be a delay of 10 seconds)
D. Attack · mouse position: 【increase output power and consume 3 times of power】 After confirming the starting position of the attack, the Drakken Laser Drill will follow the mouse position to emit laser, causing damage to all objects within 1 grid around (be careful of friendly injury) (When clicking "left click" again: stop the attack. When clicking "right click": please do not do this. If you click the "right click", you need to click the stop button to stop the attack)
E. Stop: stop the attack action, unable to stop the Concentrated Beam and Pulse Cannon
F. Concentrated Beam: 【increase output power, consume 6 times of power】 attack all objects on the selected path (cause a total of 22 times of basic damage) (only cause no damage to your side)
G. Pulse Cannon: 【increase output power and consume 10 times of power】 The main laser attacks all objects in the selected and surrounding 2 grid positions, converges the secondary laser and causes damage to the enemy on the action path, and finally causes an explosion of 10 times of basic damage within the 13 grid range (46 times of basic damage in total)
H. Damage control: manually control the amount of damage output. For 7-12 damage, an additional secondary laser will be added for each+1 damage (decorative)



3. CE: Theoretically compatible, but I'm too lazy to study the damage and armor code of CE, so you need to adjust the damage and armor penetration yourself in Mod settings





Xml + Dll ：空曜
Texture：空曜，TOT

ReTexture：（mo）https://steamcommunity.com/sharedfiles/filedetails/?id=2942087033


Bug：
1.semi-random research Mod：
Problem ：will continue to upgrade when the research is completed
Solution ：Let Pawn advance the number of research progress, no matter how many）


Update:
03-07：
1. Fix some bugs that sometimes automatic attack can't work properly (for raid of drop pod )
2. (Mod setting page) added the option of shielding upgrade letters
3. Added cross-map attack function
4. Fixed the bug that the maximum armor penetration could not be saved normally

03-08：
1. Add an option in Mod setting: whether to clear the current research after the completion of circular technology (to prevent conflict with semi-random research Mod)
2. Add a damage adjustment in the "Settings" tab of the building (because the original damage adjustment conflicts with Better Sliders Mod, the damage cannot be adjusted)

03-12：
Fixed conflict with Better Sliders Mod (but the damage adjustment icon went to the building tab of "settings" )
03-14
Modified Texture path, fixed rendering bug
3-19
1.The search code for attacking all enemies has been optimized, and it should no longer appear (- 1000, - 1000). If there is no valid target, the debug message for the "Drakken Laser Drill Not Valid Target" will appear.". attacking all enemies prioritizes attacking the nearest enemy
2.Fixed a bug in cross map attacks where all enemies used cross map frame selection to attack the same code
3.fixed the error of English translation
04-16
Added a function to upgrade through building

![Image](https://i.imgur.com/PwoNOj4.png)



-  See if the error persists if you just have this mod and its requirements active.
-  If not, try adding your other mods until it happens again.
-  Post your error-log using the [Log Uploader](https://steamcommunity.com/sharedfiles/filedetails/?id=2873415404) or the standalone [Uploader](https://steamcommunity.com/sharedfiles/filedetails/?id=2873415404) and command Ctrl+F12
-  For best support, please use the Discord-channel for error-reporting.
-  Do not report errors by making a discussion-thread, I get no notification of that.
-  If you have the solution for a problem, please post it to the GitHub repository.
-  Use [RimSort](https://github.com/RimSort/RimSort/releases/latest) to sort your mods

 

[![Image](https://img.shields.io/github/v/release/emipa606/DrakkenLaserDrill?label=latest%20version&style=plastic&color=9f1111&labelColor=black)](https://steamcommunity.com/sharedfiles/filedetails/changelog/3550292796) | tags:  infinite range
