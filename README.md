# ProjetLogiciel
Un jeux en ligne créé en C# à l'aide de Monogame.

## Sources et remerciement
Inspiration pour les classes Renderable | Transform.
> https://github.com/RonenNess/MonoGame-Sprites
Myra UI Modifié
https://github.com/rds1983/Myra

## Groupe
- Léa Duvigneau
- Thomas Auriol

## Projet
Un jeu jouable en ligne ou le but sera de gagner le round contre d'autres joueurs en chacun pour sa peau.
Chaque partie sera générée procéduralement.

## Fonctionnalités
Serveur :
>- Création d'une partie
>- Lobby
>- Connexion Base de donnée

Client :
>- Connexion au serveur
>- Attente dans un lobby
>- Aller dans une partie
>- Ecran win / loose

## Client

## Server
Convention de paquet:
- Client:
>- Client response 1X
>- Client state 2X
>- Client navigation 3X

- Serveur:
>- Serveur response 1X
>- Serveur state 2X
>- Serveur navigation 3X

## Technologies
- C# .Net Core
- Monogame
- Socket
- MongoDB

## Todo
Ajout d'une convention de paquet