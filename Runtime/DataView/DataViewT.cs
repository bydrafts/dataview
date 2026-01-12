namespace Drafts.DataView
{
    public abstract class DataView<T> : DataView
    {
        private T _dataT;
        protected bool IsDestroying { get; private set; }

        public virtual T Data { get => _dataT; set => SetData(value); }

        public override object GetData() => _dataT;

        public override void SetData(object data)
        {
            if (_dataT is not null) Unsubscribe();
            _dataT = data is T v ? v : default;
            if (_dataT is not null) Subscribe();
            if (enabled) Redraw();
            base.SetData(data);
        }

        protected virtual void Subscribe() { }
        protected virtual void Unsubscribe() { }
        public virtual void Redraw() { }

        protected virtual void OnDestroy()
        {
            IsDestroying = true;
            if (_dataT != null) Unsubscribe();
            _dataT = default;
        }
    }
}