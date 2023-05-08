using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*using UniRx;

namespace JoshuaR
{
    public static class RabbitEventSystem
    {
        static 
    }

    #region EventDic

    public class RabbitEventDic : IRabbitEventDic
    {
        private Dictionary<Type,IEventManager>

        public IObservable<T> GetEvent<T>()
        {
            throw new NotImplementedException();
        }

        public void Publish<T>(T eventName)
        {
            throw new NotImplementedException();
        }
    }

    public interface IRabbitEventDic
    {
        IObservable<T> GetEvent<T>();

        void Publish<T>(T eventName);
    }
    #endregion

    #region EventManager
    public class EventId : Attribute
    {
        public EventId(int identifier)
        {
            Identifier = identifier;
        }

        public int Identifier { get; set; }

        public EventId()
        {
        }
    }

    public interface IEventManager
    {
        int EventId { get; set; }
        Type EventType { get; }
        void Publish(object evt);
    }

    public class EventManager<T> : IEventManager
    {
        private Subject

        public int EventId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Type EventType => throw new NotImplementedException();

        public void Publish(object evt)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}


*/