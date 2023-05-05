using RawDealView;
using Objects_Deck;
using Objects;
using Deck_Validation;
using Objects_Player;
using System.Text.Json;


namespace RawDeal;


public class Game
{
    private View _view;
    private string _deckFolder;
    private List<Player> players;
    private int playerTurnIndex;
    private List<Turn> turns;
    private int numberOfPlayers;
    
    
    public Game(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
        numberOfPlayers = 2;
        players = new List<Player>();
        turns = new List<Turn>();
        SetupPlayers();
    }

    public void Play()
    {
        if (!CanGameStart()) return;
        StartGame();
        PlayGame();
        CongratulateWinner();
    }

    private void PlayGame()
    {
        while (!IsThereAWinner())
        {
            Turn turn = GetTurn();
            turn.PlayTurn();
            UpdateTurnIndex();
        } 
    }

    private void StartGame()
    {
        InitializeTurns();
        PlayersFirstDrawOfCards();
    }

    private void InitializeTurns()
    {
        SetTurns();
        SetTurnsIndex();
        
    }

    private Turn GetTurn()
    {
        return turns[playerTurnIndex];
    }

    private void SetTurns()
    {
        turns.Add(CreateTurn( players[playerTurnIndex], players[1 - playerTurnIndex]));
        turns.Add(CreateTurn(players[1 - playerTurnIndex], players[playerTurnIndex]));
    }
    
    private Turn CreateTurn(Player player, Player opponent)
    {
        string superstarName = player.GetSuperstarName();
        if(superstarName == "HHH")
            return new TurnHHH(_view, player, opponent);
        else if(superstarName == "KANE")
            return new TurnKane(_view, player, opponent);
        else if(superstarName== "THE ROCK")
            return new TurnTheRock(_view, player, opponent);
        else if(superstarName == "THE UNDERTAKER")
            return new TurnUndertaker(_view, player, opponent);
        else if(superstarName == "CHRIS JERICHO")
            return new TurnJericho(_view, player, opponent);
        else if(superstarName == "MANKIND")
            return new TurnMankind(_view, player, opponent);
        else
            return new TurnStoneCold(_view, player, opponent);
    }
    
    private void CongratulateWinner()
    {
        string winnerName = GetWinnerName();
        _view.CongratulateWinner(winnerName);
    }
    
    private bool IsThereAWinner()
    {
        foreach(Player player in players)
        {
            if (player.IsDead())
                return true;
        }
        return false;
    }

    private string GetWinnerName()
    {
        foreach(Player player in players)
        {
            if (!player.IsDead())
                return player.GetSuperstarName();
        }
        return "";
    }

    private void UpdateTurnIndex()
    {
        playerTurnIndex = (playerTurnIndex + 1) % players.Count;
    }

    private void PlayersFirstDrawOfCards()
    {
        foreach (Player player in players)
        {
            int handSize = player.GetSuperstarHandSize();
            for (int i = 0; i < handSize; i++)
                player.AddCardToHandFromArsenal();
        }
    }

    private void SetTurnsIndex()
    {
        Superstar superstarPlayer1 = players[0].GetSuperstar();
        Superstar superstarPlayer2 = players[1].GetSuperstar();
        if (superstarPlayer1.SuperstarValue >= superstarPlayer2.SuperstarValue)
            playerTurnIndex = 0;
        else
            playerTurnIndex = 1;
    }

    private bool CanGameStart()
    {
        return !AreThereInvalidDecks();
    }
     
     private void SetupPlayers()
     { 
         for(int i = 0; i<numberOfPlayers; i++)
        {
            CreateGamePlayer();
            if (!AreThereInvalidDecks()) continue;
            _view.SayThatDeckIsInvalid();
            break;
        }
    }

     private Deck GetDeckFromUser()
     {
         string deckFileSelected = _view.AskUserToSelectDeck(_deckFolder);
         return GetDeckFromFile(deckFileSelected);
     }
     
    private bool AreThereInvalidDecks()
    {
        foreach(Player player in players)
            if (!IsDeckValid(player.GetDeck()))
                return true;
        return false;
    }  
    
    private bool IsDeckValid(Deck deck)
    {
        List<Superstar> superStars = GetSuperstars();
        DeckValidation deckValidation = new DeckValidation(deck, superStars);
        return deckValidation.CheckIfDeckIsValid();
    }

    private void CreateGamePlayer()
    {
        Deck playerDeck = GetDeckFromUser();
        Player newPlayer = new Player(playerDeck);
        AddPlayerToGamePlayers(newPlayer);
    }

    private void AddPlayerToGamePlayers(Player player)
    {
        players.Add(player);
    }

    private Deck GetDeckFromFile(string deckFile)
     {
         List<string> deckStrList = File.ReadAllText(deckFile).Split("\n").ToList(); 
         Deck deck = CreateDeck(deckStrList); 
         return deck;
     }
     private Deck CreateDeck(List<string> deckList)
     {
         int superstarIndexInDeckList = 0;
         string deckSuperstarName = deckList[superstarIndexInDeckList];
         List<Card> deckCards = GetDeckCards(deckList);
         Superstar deckSuperstar = GetDeckSuperstarCard(deckSuperstarName);
         return new Deck(deckCards, deckSuperstar);;
     }

     private List<Card> GetDeckCards(IReadOnlyList<string> deckList)
     {
         List<Card> cards = GetCards();
         List<Card> deckCards = new List<Card>();
         for (int i = 1; i<deckList.Count; i++)
         {
             Card card = cards.Find(c => c.Title == deckList[i]);
             deckCards.Add(card);
         }  
         return deckCards;
     }

     private Superstar GetDeckSuperstarCard(string superstarName)
     {
         List<Superstar> superstars = GetSuperstars();
         return superstars.Find(s => superstarName.Contains(s.Name));
     }

     private List<Card> GetCards()
     {
         string pathCards = Path.Combine("data", "cards.json");
         string jsonString = File.ReadAllText(pathCards);
         List<Card> cards = JsonSerializer.Deserialize<List<Card>>(jsonString);
         return cards;
     }

     private List<Superstar> GetSuperstars()
     {
         string pathSuperstars = Path.Combine("data", "superstar2.json");
         string jsonString = File.ReadAllText(pathSuperstars);
         List<Superstar> superstars = JsonSerializer.Deserialize<List<Superstar>>(jsonString);
         return superstars;
     }
     
}