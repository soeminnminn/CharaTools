using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Resources;
using CharaTools.AIChara;
using Newtonsoft.Json;

namespace CharaTools.HS2
{
    public static class GlobalHS2Calc
    {
        private static List<T> LoadAllFolder<T>(List<string> lstABName)
        {
            var list = new List<T>();
            

            for (int i = 0; i < lstABName.Count; i++)
            {
                try
                {
                    string json = string.Empty;
#if NET
                    var resUri = new Uri($"/Resources/{lstABName[i]}", UriKind.Relative);

                    StreamResourceInfo info = Application.GetResourceStream(resUri);
                    using (StreamReader reader = new StreamReader(info.Stream))
                    {
                        json = reader.ReadToEnd();
                    }
#else
                    var assm = Assembly.GetExecutingAssembly();
                    string[] resNames = assm.GetManifestResourceNames();
                    string resourceName = resNames.Single(str => str.EndsWith(lstABName[i]));

                    if (!string.IsNullOrEmpty(resourceName))
                    {
                        using (Stream stream = assm.GetManifestResourceStream(resourceName))
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            json = reader.ReadToEnd();
                        }
                    }
#endif

                    if (!string.IsNullOrEmpty(json))
                    {
                        var data = JsonConvert.DeserializeObject<T>(json);
                        list.Add(data);
                    }
                }
                catch (Exception)
                { }
            }
            return list;
        }

        public static IReadOnlyDictionary<int, GameParameterInfo.Param> infoGameParameterTable { get; private set; }

        private static void LoadGameParameterInfo()
        {
            if (infoGameParameterTable != null) return;

            Dictionary<int, GameParameterInfo.Param> dic = new Dictionary<int, GameParameterInfo.Param>();
            LoadAllFolder<GameParameterInfo>(new List<string>() { "gp-30-min.json", "gp-50-min.json" }).ForEach(delegate (GameParameterInfo a)
            {
                a.param.ForEach(delegate (GameParameterInfo.Param p)
                {
                    dic[p.id] = p;
                });
            });
            infoGameParameterTable = dic.ToDictionary((KeyValuePair<int, GameParameterInfo.Param> p) => p.Key, (KeyValuePair<int, GameParameterInfo.Param> p) => p.Value);
        }

        public static IReadOnlyDictionary<int, PersonalParameterInfo.Param> infoPersonalParameterTable { get; private set; }
        private static void LoadPersonalParameterInfo()
        {
            if (infoPersonalParameterTable != null) return;
            Dictionary<int, PersonalParameterInfo.Param> dic = new Dictionary<int, PersonalParameterInfo.Param>();

            LoadAllFolder<PersonalParameterInfo>(new List<string>() { "pp-30-min.json", "pp-50-min.json" }).ForEach(delegate (PersonalParameterInfo a)
            {
                a.param.ForEach(delegate (PersonalParameterInfo.Param p)
                {
                    dic[p.id] = p;
                });
            });
            infoPersonalParameterTable = dic.ToDictionary((KeyValuePair<int, PersonalParameterInfo.Param> p) => p.Key, (KeyValuePair<int, PersonalParameterInfo.Param> p) => p.Value);
        }

        static GlobalHS2Calc()
        {
            LoadGameParameterInfo();
            LoadPersonalParameterInfo();
        }

        public static int AnalAndPain(ChaFileGameInfo2 _info)
        {
            if (_info == null)
            {
                return 0;
            }
            bool flag = !_info.genericAnalVoice && _info.resistAnal >= 100;
            bool flag2 = !_info.genericPainVoice && _info.resistPain >= 100;
            if (flag && !flag2)
            {
                return 1;
            }
            if (!flag && flag2)
            {
                return 2;
            }
            if (flag && flag2)
            {
                return RandomRange(1, 3);
            }
            return 0;
        }

        public static bool IsPeepingFound(ChaFileGameInfo2 _param, int _personality, bool _isDependece = false)
        {
            if (_param == null)
            {
                return false;
            }
            if (_isDependece && _param.nowDrawState == ChaFileDefine.State.Dependence)
            {
                return true;
            }
            if (!infoPersonalParameterTable.TryGetValue(_personality, out var value))
            {
                return false;
            }
            return RandomRange(0, 100) < value.warnings[(int)_param.nowDrawState];
        }

        private static bool CalcNowDependence(ChaFileGameInfo2 _param, int _personality)
        {
            if (_param == null)
            {
                return false;
            }
            if (_param.resistH < 100)
            {
                return false;
            }
            if (_param.nowDrawState != 0 && _param.nowDrawState != ChaFileDefine.State.Aversion)
            {
                return _param.nowDrawState != ChaFileDefine.State.Broken;
            }
            return false;
        }

        private static void CalcParameter(GameParameterInfo.Param _value, ChaFileGameInfo2 _param, int _personality)
        {
            if (_param != null && _value != null && _param.hCount != 0)
            {
                if (_param.nowDrawState != ChaFileDefine.State.Broken && _param.nowDrawState != ChaFileDefine.State.Dependence && !_param.lockNowState)
                {
                    _param.Favor += RandomRange(_value.favor.min, _value.favor.max + 1);
                    _param.Enjoyment += RandomRange(_value.enjoyment.min, _value.enjoyment.max + 1);
                    _param.Slavery += RandomRange(_value.slavery.min, _value.slavery.max + 1);
                    _param.Aversion += RandomRange(_value.aversion.min, _value.aversion.max + 1);
                }

                _param.Dirty += RandomRange(_value.dirty.min, _value.dirty.max + 1);
                _param.Tiredness += RandomRange(_value.tiredness.min, _value.tiredness.max + 1);
                _param.Toilet += RandomRange(_value.toilet.min, _value.toilet.max + 1);
                _param.Libido += RandomRange(_value.libido.min, _value.libido.max + 1);

                if (!_param.lockBroken)
                {
                    _param.Broken += RandomRange(_value.broken.min, _value.broken.max + 1);
                }

                if (CalcNowDependence(_param, _personality) && !_param.lockDependence)
                {
                    _param.Dependence += RandomRange(_value.dependence.min, _value.dependence.max + 1);
                }

                if (!_param.isChangeParameter)
                {
                    _param.isChangeParameter = true;
                }
            }
        }

        public static void CalcParameter(int _num, ChaFileGameInfo2 _param, int _personality)
        {
            if (_param != null && infoGameParameterTable.TryGetValue(_num, out var value))
            {
                CalcParameter(value, _param, _personality);
            }
        }

        public static void CalcParameterH(List<GameParameterInfo.Param> _values, ChaFileGameInfo2 _param, int _personality)
        {
            if (_param == null || _values == null || _values.Count == 0)
            {
                return;
            }

            foreach (GameParameterInfo.Param _value in _values)
            {
                CalcParameter(_value, _param, _personality);
            }
        }

        public static void CalcParameterH(GameParameterInfo.Param _value, ChaFileGameInfo2 _param, int _personality)
        {
            if (_param != null && _value != null)
            {
                CalcParameter(_value, _param, _personality);
            }
        }

        public static void CalcState(ChaFileGameInfo2 _param, int _personality)
        {
            if (_param == null)
            {
                return;
            }

            if (_param.nowDrawState == ChaFileDefine.State.Broken)
            {
                if (_param.Broken > 0)
                {
                    return;
                }
            }
            else if (_param.nowDrawState == ChaFileDefine.State.Dependence && _param.Dependence > 0)
            {
                return;
            }

            List<(ChaFileDefine.State, int)> list = new List<(ChaFileDefine.State, int)>
            {
                (ChaFileDefine.State.Favor, _param.Favor),
                (ChaFileDefine.State.Enjoyment, _param.Enjoyment),
                (ChaFileDefine.State.Slavery, _param.Slavery),
                (ChaFileDefine.State.Aversion, _param.Aversion)
            };

            if (list.Any(((ChaFileDefine.State id, int state) l) => l.state >= 20))
            {
                CalcState(list, ref _param.nowState);
            }
            else
            {
                _param.nowState = ChaFileDefine.State.Blank;
            }

            if (list.Any(((ChaFileDefine.State id, int state) l) => l.state >= 50))
            {
                CalcState(list, ref _param.nowDrawState);
            }
            else
            {
                _param.nowDrawState = ChaFileDefine.State.Blank;
            }

            if (_param.Broken >= 100)
            {
                _param.nowState = ChaFileDefine.State.Broken;
                _param.nowDrawState = ChaFileDefine.State.Broken;
            }
            else if (_param.Dependence >= 100)
            {
                _param.nowState = ChaFileDefine.State.Dependence;
                _param.nowDrawState = ChaFileDefine.State.Dependence;
            }

            //if (_param.nowDrawState == ChaFileDefine.State.Favor && _param.Favor >= 100)
            //{
            //    SaveData.SetAchievementAchieve(12);
            //}
            //if (_param.nowDrawState == ChaFileDefine.State.Enjoyment && _param.Enjoyment >= 100)
            //{
            //    SaveData.SetAchievementAchieve(13);
            //}
            //if (_param.nowDrawState == ChaFileDefine.State.Slavery && _param.Slavery >= 100)
            //{
            //    SaveData.SetAchievementAchieve(14);
            //}
            //if (_param.nowDrawState == ChaFileDefine.State.Aversion && _param.Aversion >= 100)
            //{
            //    SaveData.SetAchievementAchieve(15);
            //}
            //if (_param.nowDrawState == ChaFileDefine.State.Broken)
            //{
            //    SaveData.SetAchievementAchieve(16);
            //}
            //if (_param.nowDrawState == ChaFileDefine.State.Dependence)
            //{
            //    SaveData.SetAchievementAchieve(17);
            //}

            void CalcState(List<(ChaFileDefine.State id, int state)> _list, ref ChaFileDefine.State _state)
            {
                List<(ChaFileDefine.State, int)> source = _list.MaxElementsBy(((ChaFileDefine.State id, int state) l) => l.state).ToList();
                foreach (int s in infoPersonalParameterTable[_personality].statusPrioritys)
                {
                    if (source.Any(((ChaFileDefine.State id, int state) m) => m.id == (ChaFileDefine.State)s))
                    {
                        _state = (ChaFileDefine.State)s;
                        break;
                    }
                }
            }
        }

        public static IEnumerable<TSource> MaxElementsBy<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            TResult value = source.Max(selector);
            return source.Where((TSource c) => selector(c).Equals(value));
        }

        public static int RandomRange(int min, int max)
        {
            Random r = new Random();
            return r.Next(min, max);
        }
    }
}
