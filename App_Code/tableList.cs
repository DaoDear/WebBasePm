using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebBasePM
{
    public class tableList : IComparer
    {
        public string name;
        public string[] header;
        public ArrayList rows;
        public ArrayList merge = null;
        //protected int circleIndex;

        int IComparer.Compare(object x, object y)
        {
            return string.Compare(((tableList)x).getName(), ((tableList)y).getName());
        }

        public tableList()
        {
            rows = new ArrayList();
        }

        public tableList(string n)
        {
            rows = new ArrayList();
            name = n;
        }

        public int addHeader(string[] h)
        {
            header = h;
            return h.Length;
        }

        public void removeHeader()
        {
            header = null;
        }

        // convert bool to int
        public int addRow(object[] row)
        {
            return rows.Add(row);
        }

        public string getName()
        {
            return name;
        }

        public string[] getHeader()
        {
            return header;
        }


        public string getHeader(int index)
        {
            return header[index];
        }

        public ArrayList getRows()
        {
            return rows;
        }

        public object[] getRow(int index)
        {
            if (index < rows.Count)
            {
                return (object[])rows[index];
            }
            return null;
        }

        public int getRowNumber()
        {
            return rows.Count;
        }

        // may be change case header == null
        public int getColNumber()
        {
            if (header != null)
            {
                return header.Length;
            }
            else if (rows != null && rows.Count > 0)
            {
                int max = ((object[])rows[0]).Length;
                for (int i = 1; i < rows.Count; i++)
                {
                    if (max < ((object[])rows[i]).Length)
                    {
                        max = ((object[])rows[i]).Length;
                    }
                }
                return max;
            }
            else
            {
                return 0;
            }
        }

        public void setName(string n)
        {
            name = n;
        }

        public bool addMerge(int[] m)
        {
            if (m.Length == 4)
            {
                if (merge == null)
                {
                    merge = new ArrayList();
                }
                merge.Add(m);
                return true;
            }

            return false;
        }

        public void setMerge(ArrayList ma)
        {
            merge = ma;
        }

        public void clearMerge()
        {
            if (merge != null)
            {
                merge.Clear();
                merge = null;
            }
        }

        public int[][] getMergeArray()
        {
            if (merge != null)
            {
                return (int[][])merge.ToArray(typeof(int[]));
            }
            return null;
        }

        public ArrayList getMerge()
        {
            return merge;
        }

        // Table must be have header
        public object[] search(int indexKey, string key)
        {
            if (indexKey < this.getColNumber())
            {
                int rowNum = rows.Count;

                for (int i = 0; i < rowNum; i++)
                {
                    object[] tmps = (object[])rows[i];
                    if (indexKey < tmps.Length && tmps[indexKey] is string && ((string)tmps[indexKey]).Equals(key)) // (key.Equals(tmps[indexKey]))
                    {
                        return tmps;
                    }
                }
            }

            return null;
        }

        public object[] search(string key)
        {
            return search(0, key);
        }

        public object[] searchPrefix(int indexKey, string key)
        {
            if (indexKey < this.getColNumber())
            {
                int rowNum = rows.Count;

                for (int i = 0; i < rowNum; i++)
                {
                    object[] tmps = (object[])rows[i];
                    if (indexKey < tmps.Length && tmps[indexKey] is string && ((string)tmps[indexKey]).StartsWith(key)) //(tmps[indexKey].StartsWith(key))
                    {
                        return tmps;
                    }
                }
            }

            return null;
        }

        public object[] searchPrefix(string key)
        {
            return searchPrefix(0, key);
        }

        public object searchField(int indexKey, string key, int indexField)
        {
            if (indexKey < this.getColNumber())
            {
                int rowNum = rows.Count;

                if (indexField < this.getColNumber()) //this.getColNumber()
                {
                    for (int i = 0; i < rowNum; i++)
                    {
                        object[] tmps = (object[])rows[i];
                        if (indexKey < tmps.Length && indexField < tmps.Length && tmps[indexKey] is string && ((string)tmps[indexKey]).Equals(key))
                        {
                            return tmps[indexField];
                        }
                    }
                }
            }

            return null;
        }

        public object searchField(int indexKey, string key, string field)
        {
            for (int i = 0; i < header.Length; i++)
            {
                if (header[i].Equals(field))
                {
                    return searchField(indexKey, key, i);
                }
            }

            return null;
        }

        public object searchField(string indexKey, string key, int indexField)
        {
            for (int i = 0; i < header.Length; i++)
            {
                if (header[i].Equals(indexKey))
                {
                    return searchField(i, key, indexField);
                }
            }

            return null;
        }

        public object searchField(string indexKey, string key, string field)
        {
            for (int i = 0; i < header.Length; i++)
            {
                if (header[i].Equals(indexKey))
                {
                    return searchField(i, key, field);
                }
            }

            return null;
        }

        public object searchField(string key, int indexField)
        {
            return searchField(0, key, indexField);
        }

        public object searchField(string key, string field)
        {
            return searchField(0, key, field);
        }

        public object searchPrefixField(int indexKey, string key, int indexField)
        {
            if (indexKey < this.getColNumber())
            {
                int rowNum = rows.Count;

                if (indexField < this.getColNumber()) //this.getColNumber()
                {
                    for (int i = 0; i < rowNum; i++)
                    {
                        object[] tmps = (object[])rows[i];
                        if (indexKey < tmps.Length && indexField < tmps.Length && tmps[indexKey] is string && ((string)tmps[indexKey]).StartsWith(key)) //(tmps[indexKey].StartsWith(key))
                        {
                            return tmps[indexField];
                        }
                    }
                }
            }

            return null;
        }

        public object searchPrefixField(string key, int indexField)
        {
            return searchPrefixField(0, key, indexField);
        }


        public void rearrangeNumber(int col)
        {
            if (col < this.getColNumber())
            {
                int rowNum = this.getRowNumber();
                for (int i = 0; i < rowNum; i++)
                {
                    string[] strtmps = (string[])this.rows[i];          /// Make sure focused col is all string in every row
                    if (col < strtmps.Length && strtmps[col] != null)
                    {
                        int tmpInt;
                        bool isNumber = int.TryParse(strtmps[col], out tmpInt);

                        /* rearrange */
                        if (isNumber)
                        {
                            strtmps[col] = tmpInt.ToString();
                            /*strtmps[col] = string.valueOf(chs, 0, last);*/
                        }
                    }
                }
            }
        }

        public void rearrangeNumberDouble(int col)
        {
            if (col < this.getColNumber())
            {
                int rowNum = this.getRowNumber();
                for (int i = 0; i < rowNum; i++)
                {
                    string[] strtmps = (string[])this.rows[i];          /// Make sure focused col is all string in every row
                    if (col < strtmps.Length && strtmps[col] != null)
                    {
                        double tmpDouble;
                        bool isNumber = double.TryParse(strtmps[col], out tmpDouble);

                        /* rearrange */
                        if (isNumber)
                        {
                            strtmps[col] = tmpDouble.ToString();
                            /*strtmps[col] = string.valueOf(chs, 0, last);*/
                        }
                    }
                }
            }
        }

        public bool equals(string n)
        {
            //System.out.println (n + "-vs-" + this.getName());
            return n.Equals(this.getName());
        }

        public bool equals(tableList t)
        {
            //System.out.println (name + "-VS-" + t.getName());
            return t.getName().Equals(name);
        }

        public override string ToString()
        {
            string strtmp = "";
            if (name != null)
            {
                strtmp = "\tTable Name : " + name + "\n";
            }
            if (header != null)
            {
                for (int i = 0; i < header.Length; i++)
                {
                    strtmp += header[i] + "\t";
                }
                strtmp += "\n";
            }
            if (rows != null)
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    object[] tmps = (object[])rows[i];
                    for (int j = 0; j < tmps.Length; j++)
                    {
                        if (tmps[j] is string)
                        {
                            strtmp += ((string)tmps[j]) + "\t";
                        }
                        else if (tmps[j] is tableList)
                        {
                            strtmp += "[TABLE]:" + ((tableList)tmps[j]).getName() + "\t";
                        }
                        else
                        {
                            strtmp += "[UNKNOWN]\t";
                        }
                    }
                    strtmp += "\n";
                }
            }
            return strtmp;
        }
    }

    public class SetOfTableList
    {
        private ArrayList tables;

        public SetOfTableList()
        {
            tables = new ArrayList();
        }

        /* return change from bool(java) to int(C#) */
        public int addTableList(tableList t)
        {
            return tables.Add(t);
        }

        /* may adapt to sort before search later */
        public tableList getTableList(string tableName)
        {
            for (int i = 0; i < tables.Count; i++)
            {
                if (((tableList)tables[i]).getName().Equals(tableName))
                {
                    return (tableList)tables[i];
                }
            }
            return null;
        }
    }
}
