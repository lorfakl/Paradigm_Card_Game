using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

public class TerritoryChallengeState : State, IState
{
    Location player1Temp;
    Location player2Temp;

    public TerritoryChallengeState()
    {
        this.Player1 = GameMaster.PlayerOne;
        this.Player2 = GameMaster.PlayerTwo;
        player1Temp = new Location("Temp", (Player)Player1);
        player2Temp = new Location("Temp", (Player)Player2);

        Debug.Log("Territory Challenge State initialized");
    }

    public bool IsUniqueToPlayer()
    {
        return false;
    }

    public void OnEntry()
    {
        HelperFunctions.Print("T0 Transport: TerrirtoyChallenge START");
    }

    public void OnExit()
    {
        if(player1Temp == null || player2Temp == null)
        {
            throw new System.Exception("One of the temp locations is null my guess is AI");
        }

        HelperFunctions.Print("Player 2 Temp Size: " + player2Temp.Count);
        HelperFunctions.Print("Player 2 Temp Data: " + player2Temp.SelectContent());
        HelperFunctions.Print("Player 2 Type: " + player2Temp.Owner.Type);
        HelperFunctions.Print("Player 1 Temp Size: " + player1Temp.Count);
        HelperFunctions.Print("Player 1 Temp Data: " + player1Temp.SelectContent());
        List<IPlayable> playerTurnOrder = StartTerritoryChallenge(player1Temp.SelectContent(), 
                                                                  player2Temp.SelectContent());
        GameMaster.SetTurnOrder(playerTurnOrder);
        HelperFunctions.Print("T0 Transport: TerrirtoyChallenge END");
    }

    public async Task Operation()
    {
        
        //Debug.Log("Start Player 1 Operation");
        await this.Player1.ChooseTerritoryChallengeCard(player1Temp);
        //Debug.Log("Awaiting? Player 1 Operation");
        //Debug.Log("Start Player 2 Operation");
        await this.Player2.ChooseTerritoryChallengeCard(player2Temp);
        //Debug.Log("Awaiting? Player 2 Operation");
    }

    public void SetOwner(Player p)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    public List<IPlayable> StartTerritoryChallenge(Card p1Pick, Card p2Pick)
    {
        List<IPlayable> playerTurnOrder = new List<IPlayable>();

        //throw new Exception("The guts havent been made yet these card objects are hella null");

        if (Card.GetShape(p1Pick) > Card.GetShape(p2Pick))
        {
            if (Card.GetShape(p1Pick) == ShapeTrait.Triangle)
            {
                playerTurnOrder.Add(Player2); //Player2 loses TC goes first
                playerTurnOrder.Add(Player1);
                return playerTurnOrder;
            }
            else
            {
                playerTurnOrder.Add(Player1);//Player1 loses TC goes first
                playerTurnOrder.Add(Player2);
                return playerTurnOrder;
            }
        }
        else if (Card.GetShape(p2Pick) == ShapeTrait.Triangle)
        {
            playerTurnOrder.Add(Player1);//Player1 loses TC goes first
            playerTurnOrder.Add(Player2);
            return playerTurnOrder;
        }
        else if (Card.GetShape(p1Pick) == Card.GetShape(p2Pick))
        {
            int num = UnityEngine.Random.Range(0, 100);
            if (num > 50)
            {
                playerTurnOrder.Add(Player1);//Player1 loses TC goes first
                playerTurnOrder.Add(Player2);
                return playerTurnOrder;
            }
            else
            {
                playerTurnOrder.Add(Player2);//Player2 loses TC goes first
                playerTurnOrder.Add(Player1);
                return playerTurnOrder;
            }
        }
        else
        {
            playerTurnOrder.Add(Player2);//Player2 loses TC goes first
            playerTurnOrder.Add(Player1);
            return playerTurnOrder;
        }

    }
}
