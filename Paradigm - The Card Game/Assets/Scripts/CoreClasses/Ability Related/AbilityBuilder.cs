using System;
using System.Collections;
using HelperFunctions;
using System.Reflection;
using System.Collections.Generic;
using System.Security.Permissions;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataBase;
using System.Linq;

namespace Builder
{
    public static class AbilityBuilder
    {
        static List<Condition> cndAll = new List<Condition>();
        static List<Action> actsAll = new List<Action>();
        public static List<Ability> abilities = new List<Ability>();

        public static bool isJSONDataLoaded = false;

        public static void CreateAbilityInstance(Card c)
        {
            if(!isJSONDataLoaded)
            {
                ParseAbilityJSON();
            }

            foreach(var a in abilities)
            {
                if(a.OwningCardName == c.Name)
                {
                    try
                    {
                        Ability newAbl = new Ability(a);
                        c.Abilities.Add(newAbl);
                        newAbl.LinkToCard(c);
                    }
                    catch(Exception ex)
                    {
                        HelperFunctions.CatchException(ex);
                        HelperFunctions.Print(a.OwningCardName);
                    }
                    
                    
                }
            }
        }

        public static void ParseAbilityJSON()
        {
            if(!isJSONDataLoaded)
            {
                string path = "Assets/JSONAbilityLanguage.json";
                StreamReader read = new StreamReader(path);
                var o = JObject.Parse(read.ReadToEnd());
                //HelperFunctions.Print(o.ToString());
                //HelperFunctions.Print(o["Cards"].ToString());

                string[] infoFromFile = { "Name", "ClassType", "ID" };

                foreach (var item in o["Cards"])
                {
                    //HelperFunctions.Print(item["Name"]);
                    foreach (var abl in item["Abilities"])
                    {

                        List<Condition> cnd = new List<Condition>();
                        List<Action> acts = new List<Action>();

                        //HelperFunctions.Print("Reading the Conditions");
                        //HelperFunctions.Print(item.ToString());
                        foreach (var cond in abl["Conditions"])
                        {
                            //HelperFunctions.Print(cond.ToString());
                            Condition c = JsonConvert.DeserializeObject<Condition>(cond.ToString());
                            cnd.Add(c);
                            cndAll.Add(c);
                            //HelperFunctions.PrintObjectProperties<Condition>(c);
                        }

                        //HelperFunctions.Print("Reading theActions");
                        foreach (var act in abl["Actions"])
                        {
                            //HelperFunctions.Print(act.ToString());
                            Action a = JsonConvert.DeserializeObject<Action>(act.ToString());
                            acts.Add(a);
                            actsAll.Add(a);
                            //HelperFunctions.PrintObjectProperties<Action>(a);
                        }

                        Ability ability = new Ability(cnd, acts);
                        try
                        {
                            ability.Name = item["Name"] + " " + abl["Name"].ToString();
                        }
                        catch (Exception ex)
                        {
                            HelperFunctions.Print("There's a Name missing?");
                            HelperFunctions.CatchException(ex);
                        }

                        try
                        {
                            ability.IsLimited = JsonConvert.DeserializeObject<bool>(abl["IsLimited"].ToString());
                        }
                        catch (Exception ex)
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
                isJSONDataLoaded = true;
            }
            
        }

    }
    public enum Target { Any, LocationBased, ClassType, Query}
   
    public class TargetAction
    {
        [JsonProperty(PropertyName = "EventName")]
        public string EventName { get; private set; }
        [JsonProperty]
        public Target Targets { get; private set; }
        [JsonProperty]
        public string ClassType { get; private set; }
        [JsonProperty]
        public string Location { get; private set; }
        [JsonProperty]
        public string Query { get; private set; }

        [JsonProperty(PropertyName = "Search Criteria")]
        public SearchCriteria SearchCriteria { get; private set; }

    }

    public class Criterion
    {
        private enum Op { Equal, Not, Contains }

        [JsonProperty(PropertyName = "CardInfo")]
        public string CardInfo { get; set; }

        [JsonProperty(PropertyName = "Operation")]
        public string Operation { get; set; }

        [JsonProperty(PropertyName = "Operand")]
        public string Operand { get; set; }

        public bool CheckIfCardMeetsCriteria(Card c)
        { 
            switch (HelperFunctions.ParseEnum<Op>(Operation))
            {
                case Op.Contains:
                    if (c[CardInfo].ToString().Contains(Operand)) //doing the operation
                    {
                        return true;
                    }
                    break;
                case Op.Equal:
                    if (c[CardInfo].ToString() == Operand) //doing the operation
                    {
                        return true;
                    }
                    break;
                case Op.Not:
                    if (c[CardInfo].ToString() != Operand) //doing the operation
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }
    }

    public class SearchCriteria
    {
        public enum QueryModifier { Separate, OR, AND, Null };
        private enum Op { Equal, Not, Contains}

        [JsonProperty(PropertyName = "SearchOperation")]
        public object SearchOperation { get; set; }

        public QueryModifier QueryMod { get; private set; }
        

        [JsonProperty(PropertyName = "Criteria")]
        public List<Criterion> Criteria { get; set; }

        public SearchCriteria()
        {

          
        }


        public static bool FindCardInfo(SearchCriteria searchCriteria, List<Card> cardsFromEvent)
        {
            bool isCriteriaMet = false;

            foreach (var c in cardsFromEvent)
            {
                foreach (var criteria in searchCriteria.Criteria)
                {
                    isCriteriaMet = criteria.CheckIfCardMeetsCriteria(c);
                    if (searchCriteria.QueryMod == QueryModifier.AND && !isCriteriaMet)
                    {//if the query is an AND operation and a single criteria is not met
                     //exit the loop and move to the next card
                        break;
                    }


                }
            }

            if (searchCriteria.QueryMod == QueryModifier.AND && isCriteriaMet)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public static List<Card> FindandReturnCards(SearchCriteria searchCriteria, Location source)
        {
            List<Card> results = new List<Card>();
            bool isCriteriaMet = false;
            foreach(Card c in source)
            {
                foreach (var criteria in searchCriteria.Criteria)
                {
                    isCriteriaMet = criteria.CheckIfCardMeetsCriteria(c);
                    if(searchCriteria.QueryMod == QueryModifier.AND && !isCriteriaMet)
                    {//if the query is an AND operation and a single criteria is not met
                     //exit the loop and move to the next card
                        break;
                    }

                    if(isCriteriaMet)
                    {
                        results.Add(c);
                    }
                }
            }

            return results;
        }
    }

    public class Root
    {
        public SearchCriteria SearchCriteria { get; set; }
    }

    public class Source
    {
        [JsonProperty]
        public string Player { get; set; }
        [JsonProperty]
        public string Location { get; set; }
    }

    public class ActionTarget
    {
        [JsonProperty]
        public SearchCriteria SearchCriteria { get; set; }
    }

    public class Destination
    {
        [JsonProperty]
        public string Player { get; set; }
        [JsonProperty]
        public string Location { get; set; }
    }
}
