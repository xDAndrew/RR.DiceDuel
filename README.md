# RR.DiceDuel
Red Rift Studio Test Task

## Goal:

To create a fully functional solution, that allows hosting a game-session between two players that will participate in a competitive game.

## Rules of the game:

* 2 Players can match together by pressing “Play” btn.
  + Both players enters a room, created by a matchmaker service

* A session starts when both players press “Ready” btn, then a session is divided by 3 turns in which players sequentially (one after another) perform their actions.
  + After session is over, scores displayed and the winner’s name is presented on the screen. Players are offered for a rematch (to play again inside same session)

* There are 2 set of dices for each player:
  + First set of dices has 3 normal dices, with general rolling rules
  + Second set has only 1 dice, but instead of “1” it has “X” value which immediately lose the entire game and instead of “6” it has “24” value
  + Each turn, players has option to roll only 1 of his dice sets and the resulting score of the turn will be the total sum of their roll.

(OPTIONAL): create a leaderboards page, displaying list of players and their very last game outcome. Use any desired identity provider for this task.

As a candidate for this position, you should be capable of delivering the following:
* A complete solution, with an implemented backend service for matchmaking, room/session management, gameplay logic, frontend presented via git
* A hosted private demo link
* A short explanation in form of a short documentation. Primarily focus on explaining which decisions you take during the execution of this task and why.
* A proper dedicated approach, as if this would be a feature on a real live product

<aside>
💠 Keep in mind, that the attention to details and providing bulletproof solution for different user-flow scenarios are the key for this task.
For example: its assumed, that after players finished the game, the record about the the session will be properly saved in the database with the outcome and the room and session objects will be properly disposed on the runtime.
</aside>

The visual part of the front-end is absolutely non important for this task, and will be considered as an additional benefit.
