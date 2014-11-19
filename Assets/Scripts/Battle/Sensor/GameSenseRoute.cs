using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Battle
{
    public class GameSenseRoute : IMessageDriven
    {
        private Dictionary<object, IGameSensor> gameSensors = new Dictionary<object, IGameSensor>();
        private Dictionary<String, ISensorAnalyzer> analyzerIndex = new Dictionary<String, ISensorAnalyzer>();
        
        public String[] GetMessageTypes()
        {
            List<String> messageTypes = new List<String>();
            
            foreach (KeyValuePair<string, ISensorAnalyzer> analyzerInstance in analyzerIndex)
            {
                messageTypes.Add(analyzerInstance.Value.GetMessageType());
            }
            
            return messageTypes.ToArray();
        }
        
        public void OnMessageArrived(MessageContext context)
        {
            if (context.ContainsKey(MessageParamKey.TARGETS))
            {
                List<BattleAgent> targets = (List<BattleAgent>)context.GetConetextValue(MessageParamKey.TARGETS);
                
                foreach (BattleAgent target in targets)
                {
                    if (target != null)
                    {
                        IGameSensor sensor = null;
                        gameSensors.TryGetValue(target, out sensor);
                        
                        if (sensor != null)
                        {
                            ISensorAnalyzer analyzer = null;
                            analyzerIndex.TryGetValue(context.messageType, out analyzer);
                            
                            analyzer.DoAnalyze(sensor, context);
                        }
                    }
                }
            }
            else
            {
                object target = (object)context.GetConetextValue(MessageParamKey.TARGET);
                
                if (target != null)
                {
                    IGameSensor sensor = null;
                    gameSensors.TryGetValue(target, out sensor);
                    
                    if (sensor != null)
                    {
                        ISensorAnalyzer analyzer = null;
                        analyzerIndex.TryGetValue(context.messageType, out analyzer);
                        
                        analyzer.DoAnalyze(sensor, context);
                    }
                }
            }
        }
        
        public void RegistryGameSensor(IGameSensor gameSensor)
        {
            gameSensors.Add(gameSensor.Agent, gameSensor);
        }
        
        public void UnregistryGameSensor(IGameSensor gameSensor)
        {
            gameSensors.Remove(gameSensor.Agent);
        }

        public void RegisterSensorAnalyzer(ISensorAnalyzer sensorAnalyzer)
        {
            analyzerIndex.Add(sensorAnalyzer.GetMessageType(), sensorAnalyzer);
        }
    }
}
