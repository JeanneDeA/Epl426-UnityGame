# Asylum Survival – Unity Zombie Wave Shooter

## Game Overview

**Asylum Survival** is a first-person, wave-based zombie survival game developed in **Unity**. Set inside an abandoned asylum hidden deep in a forest, the player must survive endless waves of enemies with limited resources and no escape. The environment, enemies, and escalating difficulty are designed to create constant tension and fast-paced gameplay.

---

## Background Story

Hidden deep in the forest, an abandoned asylum was forgotten after a series of unexplained deaths. You wake up alone inside its decaying halls, emergency lights flickering and screams echoing through the darkness. The radio is dead—no help is coming.

They attack in waves. Former patients, staff, and something far worse roam the asylum, drawn to sound and movement. Weapons and explosives lie scattered from a failed evacuation, offering only temporary hope.

There is no escape—only survival. Hold out as long as you can, and don’t let the asylum claim you.

---

## Controls

### Movement & Camera

* **W / A / S / D** – Move forward / left / back / right
* **Mouse** – Look around (camera rotation)
* **Space** – Jump
* **Left Shift** – Sprint / Run
* **Left Ctrl** – Crouch

### Combat

* **Left Mouse Button (LMB)** – Fire weapon
* **Right Mouse Button (RMB)** – Aim down sights

### Interaction & Items

* **E** – Interact / Pick up items (weapons, ammo, health packs, grenades, smoke grenades)
* **X** – Drop current weapon
* **G** – Throw grenade
* **T** – Throw smoke grenade

---

## Pickups & Power-Ups

Weapons, ammunition, health packs, grenades, and smoke grenades are placed throughout the asylum and can be collected via interaction.

### Dynamic Pickup Respawn System

* Items can respawn on selected waves
* Original spawn locations are tracked
* Respawn timing is configurable per wave
* Enables long-term survival and balanced difficulty pacing

---

## Core Gameplay Systems

###  Health & Damage System

* Player and zombies have independent health values
* Different weapons deal different damage
* Zombies damage the player through close-range attacks

**UI Feedback**

* A clear, high-quality health bar provides instant combat feedback

---

###  Weapon, Projectile & Impact System

* Weapons fire physical projectile bullets using Unity Physics
* Impact behavior depends on collision type:

  * Zombie hit → Blood impact effect
  * Environment hit → Surface particle effect
* All weapon animations were created in-house

---

###  Zombie AI State Machine

Zombies are controlled by a custom AI state machine:

* **Patrol State** – Roams the environment
* **Chase State** – Activates when the player is detected (line-of-sight/awareness)

This system allows dynamic and reactive enemy behavior.

---

###  Inventory System

* Weapon switching support
* Storage for grenades and smoke grenades
* Ammo storage for long-term survival strategy

---

###  Zombie Wave Spawner

* Endless wave-based spawning system
* Increasing difficulty per wave
* Supports multiple spawn locations
* Easily expandable for new levels and challenges

---

###  Dynamic Pickup Respawn System

* Tracks original spawn points
* Allows designers to specify respawn waves
* Enhances replayability and strategic decision-making


##  Responsibilities

### Pantelis Kanaris

* Inventory System
* Dynamic Pickup Respawn System
* Custom Zombie Wave Spawner
* Player Movement & Camera
* Interaction & Item Handling

### Ioanna Hadjiandreou

* Zombie AI State Machine
* User Interface
* Health & Damage System
* Weapon Animations (50%)
* Scene Creation & Setup

---

##  Engine & Tools

* **Game Engine:** Unity
* **Programming Language:** C#
* **Genre:** First-Person Survival / Zombie Wave Shooter

---

##  Notes

This project focuses on modular systems, replayability, and scalable difficulty, making it suitable for future expansion with new enemies, weapons, and environments.
