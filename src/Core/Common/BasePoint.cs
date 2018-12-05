using System;

namespace Core
{
    [Serializable]
    public abstract class BasePoint : IPoint
    {
        private string _name;
        private string _id;
        private string _device;
        private Type _datatype;
        private object _defaultValue;

        private ReadWriteFlags _readwrite;
        private DateTime _modifyTime;
        private PointStatusFlags _status;

        private object _value;
        private object _ctrl;

        #region IDataPoint Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ValueChanged;
        public event EventHandler ControlChanged;

        public string Name
        {
            get
            {
                lock (this)
                    return _name;
            }
            set => _name = value;
        }

        public string Id
        {
            get
            {
                lock (this)
                    return _id;
            }
            set => _id = value;
        }

        public string Device
        {
            get
            {
                lock (this)
                    return _device;
            }

            set => _device = value;
        }

        public string FullName
        {
            get
            {
                lock (this)
                    return _device + "@" + _name;
            }
        }

        public Type DataType
        {
            get
            {
                lock (this)
                    return _datatype;
            }

            set => _datatype = value;
        }

        public ReadWriteFlags ReadWrite
        {
            get
            {
                lock (this)
                    return _readwrite;
            }

            set => _readwrite = value;
        }

        public object DefaultValue
        {
            get
            {
                lock (this)
                    return _defaultValue;
            }

            set => _defaultValue = value;
        }

        public virtual object Ctrl
        {
            get
            {
                lock (this)
                    return _ctrl;
            }
            set
            {
                if (_readwrite == ReadWriteFlags.ReadOnly)
                    return;

                _ctrl = value;
                ControlChanged?.Invoke(this, new EventArgs());
            }
        }

        public virtual object Value
        {
            get
            {
                if (_status == PointStatusFlags.Good)
                {
                    lock (this)
                        return _value;
                }

                lock (this)
                    return _defaultValue;
            }
            set
            {
                if (_readwrite != ReadWriteFlags.ReadOnly)
                    DoUpdate(value);
            }
        }

        public DateTime ModifyTime
        {
            get
            {
                lock (this)
                    return _modifyTime;
            }
        }

        public string Status
        {
            get
            {
                switch (_status)
                {
                    case PointStatusFlags.Good:
                        return "Good";
                    case PointStatusFlags.Bad:
                        return "Bad";
                    case PointStatusFlags.NotUsed:
                        return "NotUsed";
                    case PointStatusFlags.Unknown:
                    default:
                        return "Unknown";
                }
            }
        }

        public PointStatusFlags StatusFlags
        {
            get
            {
                lock (this)
                    return _status;
            }
            set
            {
                lock (this)
                {
                    _status = value;
                    _modifyTime = DateTime.Now;
                }
                FireValueChanged();
            }
        }

        public object Tag { get; set; }

        #endregion               
        public BasePoint(string name,
                            string device,
                              Type datatype,
                    ReadWriteFlags readwrite,
                            object defaultVal)
        {
            _name = name;
            _id = Guid.NewGuid().ToString();
            _readwrite = readwrite;
            _device = device;
            _datatype = datatype;

            if (defaultVal == null)
            {
                _defaultValue = _datatype.IsValueType ? Activator.CreateInstance(_datatype) : null;

                if (_datatype.ToString() == "System.String")
                    _defaultValue = string.Empty;
            }
            else
            {
                _defaultValue = defaultVal;
            }

            _value = _defaultValue;
            _modifyTime = DateTime.MinValue;
            _status = PointStatusFlags.Unknown;
        }

        public virtual void Reset()
        {
            object defVal;
            lock (this)
                defVal = _defaultValue;

            InternalSetValue(defVal, DateTime.Now, PointStatusFlags.Unknown);
        }

        private void FireValueChanged()
        {
            if (PropertyChanged != null)
            {
                OnPropertyChanged("Value");
                OnPropertyChanged("ModifyTime");
                OnPropertyChanged("Status");
                OnPropertyChanged("StatusFlags");
            }

            ValueChanged?.Invoke(this, new EventArgs());
        }

        private void InternalSetValue(object value, DateTime externalTime, PointStatusFlags status)
        {
            var fire = false;

            lock (this)
            {
                var old = _value;

                if (value != null)
                {
                    _value = value;
                    //_datatype = value.GetType().ToString();
                    _status = status;
                }
                else
                {
                    _value = _defaultValue;
                    //_datatype = _defaultValue.GetType().ToString();
                    _status = PointStatusFlags.Unknown;
                }

                _modifyTime = externalTime;
                if (old != null)
                    fire = !old.Equals(_value);
                else
                    fire = true;
            }
            if (fire)
                FireValueChanged();
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
        }

        public abstract void DoUpdate();

        protected virtual void DoUpdate(object value)
        {
            InternalSetValue(value, DateTime.Now, PointStatusFlags.Good);
        }

        public virtual void DoUpdate(object value, DateTime externalTime, PointStatusFlags status)
        {
            InternalSetValue(value, externalTime, status);
        }
    }
}
