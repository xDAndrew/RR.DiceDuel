using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.StateMachine.Interfaces;
using RR.DiceDuel.ExternalServices.EntityFramework;

namespace RR.DiceDuel.Core.StateMachine.States;

public class GameOngoingState : GameState
{
    private int _round;
    private int _currentPlayerTurn;

    private readonly Random _random = new(Guid.NewGuid().GetHashCode());
    
    public override void UpdateState(Session sessionContext, ref GameState nextState)
    {
        if (!IsAllPlayersConnect(sessionContext))
        {
            nextState = new FinishState();
            return;
        }

        sessionContext.CurrentState = SessionStateType.GameOngoing;
        
        // Если остался только 1 игрок который не проиграл
        if (sessionContext.PlayerStatus.Where(x => !x.IsPlayerLost).ToList().Count == 1)
        {
            nextState = new ResultCalculationState();
            sessionContext.GameLog.Push("This match is over");
        }
        
        if (_currentPlayerTurn == sessionContext.GameConfig.RoomMaxPlayer)
        {
            // раунд сыгран
            _round++;
            _currentPlayerTurn = 0;
            
            if (_round == sessionContext.GameConfig.MaxGameRound)
            {
                // игра сыграна, можно перейти к подсчету результатов
                nextState = new ResultCalculationState();
                sessionContext.GameLog.Push("This match is over");
            }
        }
        else
        {
            sessionContext.CurrentPlayerMove = sessionContext.PlayerStatus[_currentPlayerTurn].PlayerInfo.Name;
            var playerInput = sessionContext.PlayerStatus[_currentPlayerTurn].LastInput;
            
            if (string.IsNullOrWhiteSpace(playerInput))
            {
                return;
            }

            var playerResult = 0;
            switch (playerInput)
            {
                case "safe":
                    // Бросаем 3 кубика
                    var firstDice = _random.Next(1, 7);
                    var secondDice = _random.Next(1, 7);
                    var thirdDice = _random.Next(1, 7);
                    playerResult = firstDice + secondDice + thirdDice;
                    
                    sessionContext.PlayerStatus[_currentPlayerTurn].GameStatistic.NormalRolled++;
                    
                    sessionContext.GameLog.Push($"The player rolled 3 dice: [{firstDice}] [{secondDice}] [{thirdDice}]. Total {playerResult} score(s)");
                    break;
                
                case "special":
                    // Бросаем один специальный кубик
                    playerResult = _random.Next(1, 7);
                    sessionContext.PlayerStatus[_currentPlayerTurn].GameStatistic.SpecialRolled++;
                    
                    if (playerResult == 6)
                    {
                        playerResult = 24;
                        sessionContext.PlayerStatus[_currentPlayerTurn].GameStatistic.GotMaxScore++;
                    }

                    if (playerResult == 1)
                    {
                        sessionContext.PlayerStatus[_currentPlayerTurn].GameStatistic.GotZeroScore++;
                        sessionContext.PlayerStatus[_currentPlayerTurn].IsPlayerLost = true;
                    }
                    
                    sessionContext.GameLog.Push($"The player rolled 1 special dice: [{playerResult}]");
                    break;
            }
            
            sessionContext.PlayerStatus[_currentPlayerTurn].GameStatistic.TotalScores += playerResult;
            
            // Игрок походил, переходим к следующему
            sessionContext.PlayerStatus[_currentPlayerTurn].LastInput = null;
            _currentPlayerTurn++;
        }
    }
}