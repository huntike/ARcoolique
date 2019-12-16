using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

// Règles https://bit.ly/foie-glaive

public class Game : MonoBehaviour
{
    //// VARIABLES
    string Status = "playing";
    short ModalResult = -1;
    short Counter42 = 1;
    short Counter51 = 1;
    List<string> Players = new List<string>();
    List<short> PlayersBored = new List<short>();
    List<short> PlayersRetired = new List<short>();
    [SerializeField] private Dice D1;
    [SerializeField] private Dice D2;
    [SerializeField] private Button Next;
    [SerializeField] private GameObject Panel;
    [SerializeField] private Text Title;
    [SerializeField] private Text Line1;
    [SerializeField] private Text Line2;
    [SerializeField] private GameObject Btn1;
    [SerializeField] private Text Btn1Text;
    [SerializeField] private GameObject Btn2;
    [SerializeField] private Text Btn2Text;
    [SerializeField] private Text PlayerList;
    ////

    //// ROLES
    short MasterOfFate = -1;
    bool isMasterOfFate(short player) => MasterOfFate == player ? true : false;

    short God = -1;
    bool isGod(short player) => God == player ? true : false;

    short Hero = -1;
    bool isHero(short player) => Hero == player ? true : false;

    short Squire = -1;
    bool isSquire(short player) => Squire == player ? true : false;
    ////

    void Start()
    {
        Players.Add("Arthur");
        Players.Add("Félix");
        Players.Add("Logan");
        Players.Add("Margot");

        TableTurn();
    }

    //// TOUR DE TABLE
    async Task TableTurn()
    {
        short player = 0;
        short choice = -1;
        foreach (string playerName in Players)
        {
            if (PlayersRetired.Contains(player))
            {
                choice = await Modal("A la ferme", $"{playerName} est encore en vacances ?", "", "Revenir", "Rester", new List<string>());
                if (choice == 0)
                {
                    await Back(player);
                }
                player++;
                continue;
            }

            choice = await Modal("En ville", $"Que va faire {playerName} aujourd'hui ?", "", "S'aventurer", "Se retirer", new List<string>());
            if (choice == 0)
            {
                await PlayerTurn(player);
            }
            else
            {
                await Retreat(player);
            }
            player++;
        }
        await TableTurn();
    }
    ////

    //// LANCÉ DE DÉ(S)
    async Task<List<byte>> Roll(short number)
    {
        List<byte> dices = new List<byte>();
        
        if (number == 1)
        {
            D2.gameObject.SetActive(false);
            D1.transform.position = transform.TransformPoint(-0.12f, 0.05f, 0.12f);
            D1.Impulse();

            await Task.Delay(800);

            dices.Add(D1.Value());
        }
        else
        {
            D2.gameObject.SetActive(true);
            D1.transform.position = transform.TransformPoint(-0.08f, 0.05f, 0.11f);
            D1.Impulse();
            D2.transform.position = transform.TransformPoint(-0.11f, 0.05f, 0.08f);
            D2.Impulse();

            await Task.Delay(800);

            dices.Add(D1.Value());
            dices.Add(D2.Value());
        }
        return dices;
    }
    ////

    //// TOUR DE JEU
    async Task PlayerTurn(short player)
    {
        List<byte> dices = await Roll(2);
        byte d1 = dices[0] > dices[1] ? dices[0] : dices[1];
        byte d2 = dices[0] > dices[1] ? dices[1] : dices[0];

        await GetResults(player, d1, d2);
    }
    ////

    //// RÉSULTAT DU LANCÉ
    async Task GetResults(short player, byte d1, byte d2)
    {
        if ((d1 == 3 && d2 == 2) || (d1 == 4 && d2 == 1) || (d1 == 4 && d2 == 3) || (d1 == 5 && d2 == 2) || (d1 == 5 && d2 == 3) || (d1 == 5 && d2 == 4))
        {
            await Modal("Routine", "Encore une journée passé à la taverne...", $"{Players[player]} boit une gorgée", "", "", new List<string>());
            if (!PlayersBored.Contains(player))
            {
                PlayersBored.Add(player);
            }
        }
        else
        {
            if (PlayersBored.Count > 2)
            {
                string boredList = "";
                foreach (short name in PlayersBored)
                {
                    boredList += Players[name]+", ";
                }
                await Modal("Combat de Taverne", "Une bagarre éclate à la taverne !", $"{boredList}s'affrontent", "", "", new List<string>());
                await Duel(PlayersBored);
            }
            PlayersBored = new List<short>();

            if (d1 == d2)
            {
                if (d1 < 4)
                {
                    await NewHero(player, d1);
                }
                else
                {
                    await NewGod(player, d1);
                }
            }
            else if (d1 == 2 && d2 == 1)
            {
                await NewMasterOfFate(player);
            }
            else if (d1 == 3 && d2 == 1)
            {
                await NewSquire(player);
            }
            else if (d1 == 4 && d2 == 2)
            {
                await Modal("LAN au village", $"Tout le monde boit {Counter42} gorgées", "", "", "", new List<string>());
                Counter42++;
            }
            else if (d1 == 5 && d2 == 1)
            {
                await Modal("Fête au village", $"Tout le monde boit {Counter51} gorgées", "", "", "", new List<string>());
                Counter51++;
            }
            else if (d1 == 6)
            {
                await Modal("Acte de bravoure", $"{Players[player]} distribue {d2} gorgées", "", "", "", new List<string>());
            }
        }

        if (d1 + d2 == 7 && God != -1)
        {
            await DeusExMachina();
        }
    }
    ////

    //// MAITRE DU DESTIN
    async Task NewMasterOfFate(short player)
    {
        if (MasterOfFate == -1)
        {
            await Modal("Révélation", $"{Players[player]} devient Maître du Destin !", "Il pourra altérer certaines actions", "", "", new List<string>());
            MasterOfFate = player;
        }
        else
        {
            await Modal("Il ne peut en rester qu'un", $"{Players[player]} défie {Players[MasterOfFate]} pour devenir Maître du Destin", "", "Ça va chier !", "", new List<string>());
            List<short> fighters = new List<short>();
            fighters.Add(MasterOfFate);
            fighters.Add(player);

            short looser = await Duel(fighters);
            if (player != looser)
            {
                await Modal("Et ensuite ?", $"{Players[player]} devient Maître du Destin !", "Il pourra altérer certaines actions", "", "", new List<string>());
                MasterOfFate = player;
            }
        }
    }
    ////

    //// DIEU
    async Task NewGod(short player, byte gulps)
    {
        if (isGod(player))
        {
            await Modal("Acte de Bravoure", $"{Players[player]} distribue {gulps} gorgées", "", "", "", new List<string>());
        }
        else if (gulps == 6 || God == -1)
        {
            await Modal("Ascension", $"{Players[player]} devient Dieu !", $"Il distribue aussi {gulps} gorgées", "", "", new List<string>());
            God = player;
        }
        else
        {
            await Modal("Le choc des titans", $"{Players[player]} défie {Players[God]} pour devenir Dieu", "", "Ça va chier !", "", new List<string>());

            List<short> fighters = new List<short>();
            fighters.Add(God);
            fighters.Add(player);

            short looser = await Duel(fighters);
            if (player != looser)
            {
                await Modal("Et ensuite ?", $"{Players[player]} devient Dieu !", $"Il distribue aussi {gulps} gorgées", "", "", new List<string>());
                God = player;
            }
        }

        if (isHero(God))
        {
            Hero = -1;
            await NoMoreHero();
        }
        else if (isSquire(God))
        {
            Squire = -1;
        }
    }
    ////

    //// HÉROS
    async Task NewHero(short player, byte gulps)
    {
        if (isGod(player))
        {
            List<string> playerList = Players;
            playerList.RemoveAt(God);
            short squire = await Modal("Deus lo vult", $"{Players[player]} choisit quelqu'un pour devenir le Héros", $"Il distribue aussi {gulps} gorgées", "", "", playerList);
        }
        else if (isHero(player))
        {
            await Modal("Acte de bravoure", $"{Players[player]} distribue {gulps} gorgées", "", "", "", new List<string>());
        }
        else if (Hero == -1)
        {
            await Modal("Adoubement", $"{Players[player]} dévient le Héros !", $"Il distribue aussi {gulps} gorgées", "", "", new List<string>());
            Hero = player;
        }
        else
        {
            await Modal("Duel d'honneur", $"{Players[player]} défie {Players[Hero]} pour devenir Héros", "", "Ça va chier !", "", new List<string>());

            List<short> fighters = new List<short>();
            fighters.Add(Hero);
            fighters.Add(player);

            short looser = await Duel(fighters);
            if (player != looser)
            {
                await Modal("Et ensuite ?", $"{Players[player]} devient le Héros !", $"Il distribue aussi {gulps} gorgées", "", "", new List<string>());
                Hero = player;
            }
        }
    }
    ////

    //// ÉCUYER
    async Task NewSquire(short player)
    {
        if (isSquire(player))
        {
            await Modal("Acte de bravoure", $"{Players[player]} distribue 1 gorgées", "", "", "", new List<string>());
        }
        else if (isHero(player))
        {
            List<string> playerList = Players;
            playerList.RemoveAt(Hero);
            if (God != -1)
            {
                playerList.RemoveAt(God);
            }
            short squire = await Modal("Larbinnage", $"{Players[player]} choisit un joueur qui deviendra son écuyer", "", "", "", playerList);

            await Modal("Oui messire !", $"{Players[squire]} devient l'Écuyer !", $"Il prendra toujours exemple sur {Players[Hero]} en buvant autant que lui", "", "", new List<string>());
            Squire = squire;
        }
        else if (Hero == -1)
        {
            await Modal("Adoubement", $"{Players[player]} devient le Héros !", "La place était libre donc bon...", "", "", new List<string>());
            Hero = player;
        }
        else if (Squire == -1)
        {
            await Modal("Oui messire !", $"{Players[player]} devient l'Écuyer !", $"Il prendra toujours exemple sur {Players[Hero]} en buvant autant que lui", "", "", new List<string>());
            Squire = player;
        }
        else
        {
            await Modal("Combat de larbins", $"{Players[player]} défie {Players[Squire]} pour devenir l'Écuyer", "", "Ça va chier !", "", new List<string>());

            List<short> fighters = new List<short>();
            fighters.Add(Squire);
            fighters.Add(player);
            short looser = await Duel(fighters);
            if (player != looser)
            {
                await Modal("Et ensuite ?", $"{Players[player]} devient l'Écuyer !", $"Il prendra toujours exemple sur {Players[Hero]} en buvant autant que lui", "", "", new List<string>());
                Squire = player;
            }
        }

    }
    ////

    //// INTERVENTION DIVINE
    async Task DeusExMachina()
    {
        List<string> playerList = Players;
        playerList.RemoveAt(God);
        if (Hero != -1)
        {
            playerList.RemoveAt(Hero);
        }

        short innocent = await Modal("Intervention divine", $"{Players[God]} choisi un innocent à tourmenter...", "", "", "", playerList);

        List<byte> roll = await Roll(1);
        byte gulps = roll[0];
        if (Hero != -1)
        {
            await Modal("Intervention héroïque", $"{Players[Hero]} s'interpose entre {Players[innocent]} et le danger !", "", "J'ai le choix ?", "", new List<string>());

            roll = await Roll(1);
            byte score = roll[0];

            if (MasterOfFate != -1)
            {
                short influence = await Modal("Manipulaion du destin", $"{Players[MasterOfFate]} va influencer l'action du héros", "", "+1", "-1", new List<string>());
                
                if (influence == 0)
                {
                    score++;
                }
                else
                {
                    score--;
                }
            }

            if (score == 0)
            {
                await Modal("Si j'aurais su j'aurais pas v'nu", "L'innocent se défend et tabasse le Héros !", $"{Players[innocent]} boit {gulps} gorgées", "", "", new List<string>());
                await Modal("Faut que j'retourne à la ferme de mes vieux", $"{Players[Hero]} n'est plus digne d'être le héros...", $"Il boit cul-sec un verre concocté par {Players[God]}", "Mamaaann !", "", new List<string>());

                Hero = -1;
                await NoMoreHero();
            }
            else if (score == 1)
            {
                await Modal("Ohh la boulette...", "Le Héros se trompe et tabasse l'innocent !", $"{Players[innocent]} boit {gulps} gorgées", "", "", new List<string>());
                await Modal("Faut que j'retourne à la ferme de mes vieux", $"{Players[Hero]} n'est plus digne d'être le héros...", "Il fini son verre cul-sec", "Oups...", "", new List<string>());
                Hero = -1;
            }
            else if (score == 2 || score == 3)
            {
                gulps /= 2;
                await Modal("L'important c'est d'essayer", "Le Héros marche sur sa cape et tombe", $"{Players[innocent]} et {Players[Hero]} boivent {gulps} gorgées chacun", "Sans commentaire.", "", new List<string>());
            }
            else if (score == 4 || score == 5)
            {
                await Modal("En plein dans sa mouille", "Le Héros se sacrifie pour sauver l'innocent", $"{Players[Hero]} boit {gulps} gorgées", "Meh.", "", new List<string>());
            }
            else if (score == 6)
            {
                await Modal("Bourrin mais pas très malin", "Le Héros se trompe et tabasse Dieu", $"{Players[God]} boit {gulps} gorgées", "Attend... quoi ?", "", new List<string>());
            }
            else if (score == 7)
            {
                await Modal("Bourrin mais pas malin", "Le Héros se trompe et tabasse Dieu", $"{Players[God]} boit cul-sec un verre concocté par {Players[Hero]}", "Attend... quoi ?", "", new List<string>());
                await Modal("Trop vieux pour ces conneries", "Dieu prend tellement cher qu'il tombe du ciel", $"{Players[Hero]} prend sa place et devient Dieu !", "AMEN", "", new List<string>());
                God = Hero;
                Hero = -1;
                await NoMoreHero();
            }
        }
        else
        {
            await Modal("C'est la décadence hein", "L'innocent subit le châtiment divin !", $"{Players[innocent]} boit {gulps} gorgées", "", "", new List<string>());
        }
    }
    ////

    //// RETOUR A LA FERME
    async Task Retreat(short player)
    {
        await Modal("Retour à la ferme", $"{Players[player]} décide de prendre des vacances.", "Il perd ses titres s'il en avait", "", "", new List<string>());
        if (isMasterOfFate(player))
        {
            MasterOfFate = -1;
        }
        if (isGod(player))
        {
            God = -1;
        }
        if (isHero(player))
        {
            Hero = -1;
            await NoMoreHero();
        }
        if (isSquire(player))
        {
            Squire = -1;
        }
        PlayersRetired.Add(player);
    }
    ////

    //// RETOUR A L'AVENTURE
    async Task Back(short player)
    {
        await Modal("L'appel de l'aventure", $"{Players[player]} revient en ville... et commence par la taverne.", "Il boit une gorgée", "", "", new List<string>());
        PlayersRetired.Remove(player);
    }
    ////

    //// DUEL
    async Task<short> Duel(List<short> fighters)
    {
        byte minScore = 6, maxScore = 1;
        short looser = -1, winner = -1, trial = 1;
        while (winner == -1 || looser == -1)
        {
            foreach (short fighter in fighters)
            {
                await Modal(Players[fighter], $"À {Players[fighter]} de prouver sa valeur", "", "", "", new List<string>());
                List<byte> roll = await Roll(1);
                byte score = roll[0];

                if (score > maxScore)
                {
                    winner = fighter;
                    maxScore = score;
                }
                else if (score == maxScore)
                {
                    winner = -1;
                }
                else if (score == minScore)
                {
                    looser = -1;
                }
                else
                {
                    looser = fighter;
                    minScore = score;
                }
            }
            if (winner == -1 || looser == -1)
            {
                await Modal("Ex-aqueo", $"Le combat continue de faire rage", "", "", "", new List<string>());
                trial++;
            }
        }

        int gulps = fighters.Count * trial * (maxScore - minScore);
        
        await Modal("C'est fini...", $"{Players[looser]} a pris très cher !", $"Il boit {gulps} gorgées", "", "", new List<string>());

        return looser;
    }
    ////

    //// QUAND IL N'Y A PLUS DE HÉROS
    async Task NoMoreHero()
    {
        if (Squire != -1)
        {
            await Modal("Dobby est libre", $"{Players[Squire]} a pris la place du Héros", "", "", "", new List<string>());
            Hero = Squire;
            Squire = -1;
        }
    }
    ////

    //// MODAL
    async Task<short> Modal(string title, string line1, string line2, string btn1, string btn2, List<string> players)
    {
        Status = "waiting";
        if (line2 == "")
        {
            Line2.enabled = false;
        }
        else
        {
            Line2.enabled = true;
            Line2.text = line2;
        }
        if (btn1 == "")
        {
            Btn1.SetActive(false);
            Btn1Text.enabled = false;
        }
        else
        {
            Btn1.SetActive(true);
            Btn1Text.enabled = true;
            Btn1Text.text = btn1;
        }
        if (btn2 == "")
        {
            Btn2.SetActive(false);
            Btn2Text.enabled = false;
        }
        else
        {
            Btn2.SetActive(true);
            Btn2Text.enabled = true;
            Btn2Text.text = btn2;
        }

        if (btn1 != "" && btn2 != "")
        {
            Status = "waiting-for-response";
        }

        //if (players.Count == 0)
        //{
        //    PlayerList.enabled = false;
        //}
        //else
        //{
        //    Status = "waiting-for-response";
        //    PlayerList.enabled = true;
        //    PlayerList.text = line2;
        //}

        Title.text = title;
        Line1.text = line1;

        Panel.SetActive(true);
        while (Status != "playing")
        {
            await Task.Delay(500);
        }
        Panel.SetActive(false);
        return ModalResult;
    }
    ////

    //// CLICKS
    public void onClickNext()
    {
        if (Status != "waiting-for-response")
        {
            ModalResult = 0;
            Status = "playing";
        }
    }
    public void onClickBtn1()
    {
        ModalResult = 0;
        Status = "playing";
    }
    public void onClickBtn2()
    {
        ModalResult = 1;
        Status = "playing";
    }
    ////
}
