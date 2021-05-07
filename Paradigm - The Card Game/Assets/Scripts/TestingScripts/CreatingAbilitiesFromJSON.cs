using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using DataBase;
using System;
using System.Linq;
using Builder;

public class CreatingAbilitiesFromJSON : MonoBehaviour
{
    // Start is called before the first frame update\
    
    void Start()
    {
        List<Condition> cndAll = new List<Condition>();
        List<Action> actsAll = new List<Action>();
        List<Ability> abilities = new List<Ability>();

        string path = "Assets/JSONAbilityLanguage.json";
        StreamReader read = new StreamReader(path);
        var o = JObject.Parse(read.ReadToEnd());
        //print(o.ToString());
        //print(o["Cards"].ToString());

        string[] infoFromFile = { "Name","ClassType","ID"};

        foreach(var item in o["Cards"])
        {
            //print(item["Name"]);
            foreach (var abl in item["Abilities"])
            {

                List<Condition> cnd = new List<Condition>();
                List<Action> acts = new List<Action>();

                //print("Reading the Conditions");
                //print(item.ToString());
                foreach (var cond in abl["Conditions"])
                {
                    //print(cond.ToString());
                    Condition c = JsonConvert.DeserializeObject<Condition>(cond.ToString());
                    cnd.Add(c);
                    cndAll.Add(c);
                    Utilities.HelperFunctions.PrintObjectProperties<Condition>(c);
                }

                //print("Reading theActions");
                foreach (var act in abl["Actions"])
                {
                    //print(act.ToString());
                    Action a = JsonConvert.DeserializeObject<Action>(act.ToString());
                    acts.Add(a);
                    actsAll.Add(a);
                    Utilities.HelperFunctions.PrintObjectProperties<Action>(a);
                }

                Ability ability = new Ability(cnd, acts);
                try
                {
                    ability.Name = item["Name"] + " " +  abl["Name"].ToString();
                }
                catch(Exception ex)
                {
                    print("There's a Name missing?");
                    print(ex.InnerException);
                }
                
                try
                {
                    ability.IsLimited = JsonConvert.DeserializeObject<bool>(abl["IsLimited"].ToString());
                }
                catch(Exception ex)
                {
                    ability.IsLimited = false;
                }
                
                if (abl["Type"].ToString() == "Optional")
                {
                    ability.IsMandatory = false;
                }
                else
                {
                    ability.IsMandatory = true;
                }
                ability.OwningCardName = item["Name"].ToString();
                abilities.Add(ability);

            }
        }
        //print(o["Ability 2"]["Condition"][0]["EventType"]);

        Type type = typeof(string);
        Player p = new HumanPlayer(new Guid());
        CardDataBase.MakePlayerDeck(p);
        string fileInfo = infoFromFile[0];
        var cardQuery = p.GetLocation(ValidLocations.Deck).GetContents().Where(card => card[fileInfo].ToString().Contains("Dragon")).ToArray();

        //Utilities.HelperFunctions.PrintObjectProperties<Builder.Source>(actsAll[0].Source);
        //Utilities.HelperFunctions.PrintObjectProperties<Destination>(actsAll[0].Destination);
        //Utilities.HelperFunctions.PrintObjectProperties<SearchCriteria>(actsAll[0].SearchCriteria);
        foreach (var cri in actsAll[0].SearchCriteria?.Criteria)
        {
            //Utilities.HelperFunctions.PrintObjectProperties<Criterion>(cri);
        }

        print("Number of abilities created: " + abilities.Count);
        foreach(var abl in abilities)
        {
            //print(abl.OwningCardName);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static T GetPropertyValue<T>(object obj, string propName)
    {
        return (T)obj.GetType().GetProperty(propName)?.GetValue(obj, null);
    }
}
