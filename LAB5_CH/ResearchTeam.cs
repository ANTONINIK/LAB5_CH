using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace LAB5_CH
{
    [Serializable]
    class ResearchTeam : Team, INotifyPropertyChanged, IEnumerable
    {
        private string theme;

        private TimeFrame duration;

        private List<Paper> list_papers = new List<Paper>();

        private List<Person> list_persons = new List<Person>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ResearchTeam(string name, int reg_num, string theme, TimeFrame duration) : base(name, reg_num)
        {
            this.theme = theme;
            this.duration = duration;
        }
        public ResearchTeam()
        {
            name = "Команда";
            reg_num = 1;
            theme = "Тема";
            duration = TimeFrame.Year;
        }
        public DateTime LastPaper()
        {
                if (list_papers.Count != 0)
                {
                    DateTime time = list_papers[0].publictime;
                    for (int i = 0; i < list_papers.Count; i++)
                    {
                        if (list_papers[i].publictime >= time)
                        {
                            time = list_papers[i].publictime;
                        }
                    }
                    return time;
                }
                else return new DateTime();
        }

        public Team getTeamType
        {
            get
            {
                return new Team(Name, Reg_num);
            }
            set
            {
                name = value.Name;
                reg_num = value.Reg_num;
            }
        }
        public string Theme
        {
            get
            {
                return theme;
            }
            set
            {
                theme = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Theme"));
            }
        }
        public TimeFrame Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
            }
        }
        public bool this[TimeFrame frame] { get { return frame == duration; } }
        public void AddPapers(params Paper[] list)
        {
            if (list != null)
                for (int i = 0; i < list.Length; i++)
                {
                    list_papers.Add(list[i]);
                }
        }
        public void AddMembers(params Person[] list)
        {
            if (list != null)
                for (int i = 0; i < list.Length; i++)
                {
                    list_persons.Add(list[i]);
                }
        }
        public override string ToString()
        {
            string buffer1 = "\n";
            for (int i = 0; i < list_papers.Count; i++)
            {
                buffer1 = buffer1 + list_papers[i].ToString() + '\n';
            }

            string buffer2 = "\n";
            for (int i = 0; i < list_persons.Count; i++)
            {
                buffer2 = buffer2 + list_persons[i].ToString() + '\n';
            }

            return "\t" + theme + " " + name + " " + reg_num + " " + duration + " " + buffer1 + " "; //buffer2;
        }
        public virtual string ToShortString()
        {
            return theme + " " + name + " " + reg_num + " " + duration;
        }
        public override bool Equals(object obj)
        {
            return obj is ResearchTeam team &&
                   theme == team.theme &&
                   name == team.name &&
                   reg_num == team.reg_num &&
                   duration == team.duration &&
                   list_papers.Equals(team.list_papers) &&
                   list_persons.Equals(team.list_persons);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(name, theme, reg_num, duration, list_papers, list_persons);
        }
        public static bool operator ==(ResearchTeam ob1, ResearchTeam ob2)
        {
            return ob1.name == ob2.name && ob1.theme == ob2.theme && ob1.reg_num == ob2.reg_num && ob1.duration == ob2.duration && ob1.list_papers == ob2.list_papers && ob1.list_persons == ob2.list_persons;
        }
        public static bool operator !=(ResearchTeam ob1, ResearchTeam ob2)
        {
            return ob1.name != ob2.name || ob1.theme != ob2.theme || ob1.reg_num != ob2.reg_num || ob1.duration != ob2.duration || ob1.list_papers != ob2.list_papers || ob1.list_persons != ob2.list_persons;
        }
        public IEnumerable MembersWithoutPublications()
        {

            ArrayList AutorsWithoutP = new ArrayList();
            bool someBool;
            foreach (Person pers in list_persons)
            {
                someBool = true;
                foreach (Paper pap in list_papers)
                {
                    if (pap.author == pers)
                    {
                        someBool = false;
                        break;
                    }
                }
                if (someBool)
                {
                    AutorsWithoutP.Add(pers);
                }

            }
            for (int i = 0; i < AutorsWithoutP.Count; i++)
            {
                yield return (Person)AutorsWithoutP[i];
            }

        }
        public IEnumerable MembersWithPublications()
        {

            ArrayList AutorsWithoutP = new ArrayList();
            bool someBool;
            foreach (Person pers in list_persons)
            {
                someBool = false;
                foreach (Paper pap in list_papers)
                {
                    if (pap.author == pers)
                    {
                        someBool = true;
                        break;
                    }
                }
                if (someBool)
                {
                    AutorsWithoutP.Add(pers);
                }

            }
            for (int i = 0; i < AutorsWithoutP.Count; i++)
            {
                yield return (Person)AutorsWithoutP[i];
            }

        }
        public IEnumerable LastPapers(int N_years)
        {
            for (int i = 0; i < list_papers.Count; i++)
            {
                if (list_papers[i].publictime.Year >= DateTime.Now.Year - N_years)
                {
                    yield return list_papers[i];
                }
            }
        }
        public IEnumerator GetEnumerator()
        {
            return new ResearchTeamEnumerator(list_papers, list_persons);
        }
        public void SortedPublictime()
        {
            list_papers.Sort();
        }
        public void SortedName()
        {
            list_papers.Sort(new Paper());
        }
        public void SortedSurname()
        {
            list_papers.Sort(new PaperComparer());
        }

        public bool AddFromConsole()
        {
            Console.WriteLine("Paper or People?");
            bool flag = true;
            while (flag)
            {
                switch (Console.ReadLine())
                {
                    case "People":
                        {
                            Console.WriteLine("Формат ввода: Имя автора / Фамилия автора / Дата рождения(dd.mm.year)");
                            Console.WriteLine("Разделители: # - ! $ ? / ,");
                            string[] input_data = Console.ReadLine().Split('#', '-', '!', '$', '?', '/', ',');
                            if (input_data.Length != 3)
                            {
                                Console.WriteLine("Неверное количество аргументов");
                                return false;
                            }
                            else
                            {
                                try
                                {
                                    Person person = new Person();
                                    person.Name = input_data[0];
                                    person.Surname = input_data[1];
                                    List<int> birthday = new List<int>();
                                    foreach (string item in input_data[2].Split('.'))
                                    {
                                        birthday.Add(int.Parse(item));
                                    }
                                    this.AddMembers(person);
                                    return true;
                                }
                                catch
                                {
                                    Console.WriteLine("Повторите ввод");
                                    return false;
                                }
                            }
                            break;
                        }
                    case "Paper":
                        {
                            Console.WriteLine("Формат ввода: Название статьи / Имя автора / Фамилия автора / Дата рождения(dd.mm.year) / Дата публикации(dd.mm.year) ");
                            Console.WriteLine("Разделители: # - ! $ ? / ,");
                            string[] input_data = Console.ReadLine().Split('#', '-', '!', '$', '?', '/', ',');
                            if (input_data.Length != 5)
                            {
                                Console.WriteLine("Неверное количество аргументов");
                                return false;
                            }
                            else
                            {
                                try
                                {
                                    Paper paper = new Paper();
                                    paper.name = input_data[0];
                                    paper.author.Name = input_data[1];
                                    paper.author.Surname = input_data[2];
                                    List<int> birthday = new List<int>();
                                    foreach (string item in input_data[3].Split('.'))
                                    {
                                        birthday.Add(int.Parse(item));
                                    }
                                    paper.author.Birthday = new DateTime(birthday[2], birthday[1], birthday[0]);
                                    List<int> publictime = new List<int>();
                                    foreach (string item in input_data[4].Split('.'))
                                    {
                                        publictime.Add(int.Parse(item));
                                    }
                                    paper.publictime = new DateTime(publictime[2], publictime[1], publictime[0]);
                                    this.AddPapers(paper);
                                    return true;
                                }
                                catch
                                {
                                    Console.WriteLine("Повторите ввод");
                                    return false;
                                }
                            }
                        }
                    default:
                        Console.WriteLine("Неверный формат ввода");
                        return false;
                }
            }
            return true;
        }
        public bool Load(string filename) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.OpenOrCreate);
                ResearchTeam? newResearchTeam = formatter.Deserialize(fs) as ResearchTeam;
                fs.Close();
                list_persons = new List<Person>(newResearchTeam.list_persons);
                list_papers = new List<Paper>(newResearchTeam.list_papers);
                Theme = newResearchTeam.Theme;
                duration = newResearchTeam.duration;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }
        public bool Save(string filename) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.OpenOrCreate);
                formatter.Serialize(fs, this);
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }
        public static bool Load(string filename, ResearchTeam obj)
        {
            FileStream fs = null;
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                fs = new FileStream(filename, FileMode.OpenOrCreate);
                formatter.Serialize(fs, obj);
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }
        public static bool Save(string filename, ResearchTeam obj) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream? fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.OpenOrCreate);
                obj = (ResearchTeam)formatter.Deserialize(fs);
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally 
            {
                if (fs != null) fs.Close();
            }
        }
        public new object DeepCopy()
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                stream?.Close();
            }
        }
    }
}
