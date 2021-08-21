using System;
using System.Collections.Generic;

namespace ErosSocket.DebugPLC.DIDO
{
    public interface IDIDO
    {
        bool IsInitialBool { get; }
        NameBool Int { get; set; }
        NameBool Out { get; set; }

        bool ReadDO(int number);

        bool ReadDI(int number);

        bool WritDO(int intex, int inde, bool value);

        bool WritDO(int intex, bool value);
    }

    public class NameBool
    {
        public bool this[int index]
        {
            get
            {
                if (index < 0)
                {
                    return false;
                }
                try
                {
                    return Valeu[index];
                }
                catch (Exception)
                {
                }
                return false;
            }
            set
            {
                if (Valeu[index] != value)
                {
                    EventDs[index].TimeVlue = 0;
                    Valeu[index] = value;
                    EventDs[index].OnEventValueCh(value, EventDs[index].TimeVlue);
                }
                else
                {
                    EventDs[index].TimeVlue++;
                }
            }
        }

        public class EventD
        {
            public long TimeVlue;

            public event ValueCh EventValueCh;

            public void OnEventValueCh(bool value, long timeRun)
            {
                EventValueCh?.Invoke(value, timeRun);
            }
        }

        public delegate void ValueCh(bool value, long timeRun);

        public List<EventD> EventDs;
        private List<bool> Valeu { get; set; } = new List<bool>();

        public List<string> Name { get; set; } = new List<string>();

        public List<string> LinkIDs { get; set; } = new List<string>();
        public int Count { get { return Valeu.Count; } }

        public void AddCont(int indmex)
        {
            if (Valeu == null)
            {
                Valeu = new List<bool>();
            }
            if (LinkIDs == null)
            {
                LinkIDs = new List<string>();
            }
            EventDs = new List<EventD>();
            for (int i = 0; i < indmex; i++)
            {
                EventDs.Add(new EventD()); ;
            }
            Valeu.Clear();
            for (int i = 0; i < indmex; i++)
            {
                Valeu.Add(false);
            }
            if (Name == null)
            {
                Name = new List<string>();
            }
            for (int i = 0; i < indmex; i++)
            {
                if (Name.Count <= i)
                {
                    Name.Add(i.ToString());
                }
                if (LinkIDs.Count <= i)
                {
                    LinkIDs.Add(i.ToString());
                }
            }
        }
    }
}