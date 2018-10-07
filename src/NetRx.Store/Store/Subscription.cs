using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace NetRx.Store
{
    internal interface ISubscription
    {
        void OnNext(object value);
    }

    internal class Subscription<T> : ISubscription
    {
        private readonly BehaviorSubject<T> _subject;

        public Subscription(T lastValue)
        {
            _subject = new BehaviorSubject<T>(lastValue);
        }

        public void OnNext(object value)
        {
            _subject.OnNext((T)value);
        }

        public IObservable<T> AsObservable()
        {
            return _subject.AsObservable();
        }
    }
}