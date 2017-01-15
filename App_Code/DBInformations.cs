using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace WebBasePM
{
    public class OracleInformation : ReadInformation
    {
        public SetOfTableList readInputLog(TextReader reader)
        {
            SetOfTableList tables = new SetOfTableList();
            string line = null;
            string topic = null, tableName = null, strtmp = null;
            string[] strtmps = null, fields;
            int[] charInCol = null;
            int state = 0;
            int tableNum = 0;
            tableList tableTmp = null;
            /*
             * 		reading state
             *	0 = initial  
             * 	1 = already collect a table name / ready to get header of fields
             * 	2 = may be collect header of fields (tmp) / ready to get number of character of fields
             * 	3 = already collect charInCol / create a tableList / ready to get rows
             */

            while ((line = reader.ReadLine()) != null)
            {

                int c = checkCase(line);
                if (c == 1)
                {
                    topic = getCenter(line, 3, '#');
                    //System.out.println("case:1(" + topic + ")");
                    state = 0;
                }
                else if (c == 2)
                {
                    tableName = getCenter(line, 3, '-');
                    //System.out.println("case:2(" + tableName + ")");
                    tableNum = 1;
                    state = 1;
                }
                else if (c == 3)
                {
                    if (state == 3)
                    {
                        //tableTmp.printTable(); // this for check table
                        if (tableTmp != null)
                        {
                            //System.out.println(" %%%%%% tables == null %%%%%");
                            tables.addTableList(tableTmp);
                        }
                    }
                    //System.out.println("case:3");
                    state = 0;
                }
                else
                {
                    //System.out.println(line.trim());
                    if (state == 1)
                    {
                        if (line.Length > 0)
                        {
                            strtmp = line;
                            state = 2;
                        }
                    }
                    else if (state == 2)
                    {
                        // First, check delimiter between header and rows
                        string s = line.Trim();
                        bool deli = true;
                        if (s.Length == 0)
                        {
                            deli = false;
                        }
                        for (int i = 0; i < s.Length && deli; i++)
                        {
                            if (!(s[i] == '-' || s[i] == ' ') || (s[i] == ' ' && s[i - 1] != '-'))
                            {
                                deli = false;
                                //System.out.println ("It's not deli");
                            }
                        }
                        // Then create a header
                        if (deli)
                        {
                            strtmps = line.Trim().Split(new char[] { ' ' });
                            charInCol = new int[strtmps.Length];
                            fields = new string[strtmps.Length];
                            int pos = 0;
                            for (int i = 0; i < charInCol.Length; i++)
                            {
                                charInCol[i] = strtmps[i].Length;
                                Debug.WriteLine("Position = " + pos + ", i = " + i + ", Strtmp = " + strtmp);
                                fields[i] = strtmp.Substring(pos, charInCol[i]).Trim(); //fixed
                                pos += charInCol[i] + 1;
                            }
                            //for (int i = 0; i < charInCol.length; i++) {
                            //	System.out.println(fields[i]);
                            //}
                            //System.out.println(header + "@" + tableName + "@" + tableNum);

                            tableTmp = new tableList(topic + "@" + tableName + "@" + tableNum);
                            tableTmp.addHeader(fields);
                            //tableTmp.printTable();
                            tableNum++;

                            state = 3;
                        }
                        else
                        {
                            if (line.Length > 0)
                            {
                                strtmp = line;
                            }
                            else
                            {
                                state = 1;
                            }
                        }
                    }
                    else if (state == 3)
                    {
                        if (line.Length > 0)
                        {
                            //strtmps = line.trim().split(" ");
                            fields = new string[charInCol.Length];
                            int pos = 0;
                            for (int i = 0; i < charInCol.Length; i++)
                            {
                                fields[i] = rearrangeNumber(line.Substring(pos, charInCol[i]).Trim()); //fixed
                                pos += charInCol[i] + 1;
                            }
                            // consider to concat with previous?
                            if (fields[0].Equals(""))
                            {
                                for (int i = 1; i < charInCol.Length; i++)
                                {
                                    if (!fields[i].Equals(""))
                                    {
                                        strtmps[i] += (fields[i].Trim());
                                        pos += charInCol[i] + 1;
                                    }
                                }
                            }
                            else
                            {
                                strtmps = fields;
                                tableTmp.addRow(fields); ////////////////////////////////////////////////////////////////////////////////////////////////////////
                            }
                        }
                        else
                        {
                            //tableTmp.printTable(); // this for check table
                            if (tableTmp != null)
                            {
                                //System.out.println(" %%%%%% tables == null %%%%%");
                                tables.addTableList(tableTmp);
                            }
                            state = 1;
                        }
                    }
                }
            }
            return tables;
        }
        //*/
        private int checkCase(string text)
        {
            if (text.Length > 2)
            {
                string tmp = text.Substring(0, 3);
                if (tmp.Equals(">O<"))
                { //header
                    return 1;
                }
                else if (tmp.Equals("^o^"))
                { //begin table
                    return 2;
                }
                else if (tmp.Equals("*8*") || tmp.Equals("T^T"))
                { //end table
                    return 3;
                }
            }
            return 0;
        }

        public tableList readPerfMon(string path)
        {
            string line = null;
            string strtmp = null, strtmp2 = null;
            string[] strtmps = null, strtmps2 = null, fields;
            int[] charInCol = null;
            char[] deli1 = new char[] { ',', '"' };
            char[] deli2 = new char[] { '\\' };
            tableList tableTmp = null;
            tableList tableTmp2 = null;

            using (TextReader inFile = File.OpenText(Path.Combine(path, @"test.csv")))
            {
                if ((line = inFile.ReadLine()) != null)
                {
                    fields = line.Trim().Split(deli1, StringSplitOptions.RemoveEmptyEntries);

                    decimal[] sum = new decimal[fields.Length];
                    double[] min = new double[fields.Length];
                    double[] max = new double[fields.Length];
                    int[] count = new int[fields.Length];

                    bool hasEmpty = true;
                    while ((line = inFile.ReadLine()) != null && hasEmpty)
                    {
                        strtmps = line.Trim().Split(deli1, StringSplitOptions.RemoveEmptyEntries);
                        hasEmpty = false;
                        for (int i = 1; i < strtmps.Length; i++)
                        {
                            if (strtmps[i].Trim().Length == 0)
                            {
                                hasEmpty = true;
                                break;
                            }
                        }
                    }
                    if (strtmps != null)
                    {
                        for (int i = 1; i < strtmps.Length; i++)
                        {
                            sum[i] = decimal.Parse(strtmps[i]);
                            min[i] = (double)sum[i];
                            max[i] = min[i];
                            count[i] = 1;
                        }

                        while ((line = inFile.ReadLine()) != null)
                        {
                            strtmps = line.Trim().Split(deli1, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 1; i < strtmps.Length; i++)
                            {
                                decimal dTmp;
                                if (decimal.TryParse(strtmps[i], out dTmp))
                                {
                                    sum[i] += dTmp;
                                    if ((double)dTmp < min[i])
                                    {
                                        min[i] = (double)dTmp;
                                    }
                                    if ((double)dTmp > max[i])
                                    {
                                        max[i] = (double)dTmp;
                                    }
                                    count[i]++;
                                }
                            }
                        }

                        // create index
                        int[] index = new int[fields.Length];
                        for (int i = 0; i < fields.Length; i++)
                        {
                            index[i] = i;
                        }
                        indexQuickSort(fields, index, 0, fields.Length - 1);

                        tableTmp = new tableList("PM@MSSQL@PerfMon");
                        strtmps = new string[] { "Performance object", "Counter", "Min", "Max", "Avg." };
                        tableTmp.addHeader(strtmps);

                        strtmp = "";
                        int[] merge = null;
                        for (int i = 1; i < fields.Length; i++)
                        {
                            strtmps = fields[index[i]].Split(deli2, StringSplitOptions.RemoveEmptyEntries);
                            strtmps2 = new string[5];
                            if (strtmps[1].Equals(strtmp))
                            {
                                strtmps2[0] = "";
                                merge[2]++;
                            }
                            else
                            {
                                if (merge != null && merge[0] != merge[2])
                                {
                                    tableTmp.addMerge(merge);
                                    merge = null;
                                }

                                merge = new int[] { i + 1, 1, i + 1, 1 };
                                //merge = new int[4];// { i + 1, 1, i + 1, 1 };
                                //merge[0] = i + 1;
                                //merge[1] = 1;
                                //merge[2] = i + 1;
                                //merge[3] = 1;
                                strtmps2[0] = strtmps[1];
                                strtmp = strtmps[1];
                            }
                            strtmps2[1] = strtmps[2];// +merge[0] + " " + merge[1] + " " + merge[2] + " " + merge[3];
                            strtmps2[2] = rearrangeNumber(min[index[i]].ToString("#,##0.###"));
                            strtmps2[3] = rearrangeNumber(max[index[i]].ToString("#,##0.###"));
                            strtmps2[4] = rearrangeNumber((sum[index[i]] / count[index[i]]).ToString("#,##0.###"));

                            tableTmp.addRow(strtmps2);
                        }
                        if (merge != null && merge[0] != merge[2])
                        {
                            tableTmp.addMerge(merge);
                        }

                        return tableTmp;
                    }
                }
            }

            return null;
        }

        private void indexQuickSort(string[] texts, int[] index, int left, int right)
        {
            int i = left, j = right;
            string pivot = texts[index[(left + right) / 2]];

            // partition
            while (i <= j)
            {
                while (texts[index[i]].CompareTo(pivot) < 0)  //(arr[i] < pivot)
                {
                    i++;
                }
                while (texts[index[j]].CompareTo(pivot) > 0)  //(arr[j] > pivot)
                {
                    j--;
                }
                if (i <= j)
                {
                    int tmp = index[i];
                    index[i] = index[j];
                    index[j] = tmp;
                    i++;
                    j--;
                }
            }

            // recursion
            if (left < j)
            {
                indexQuickSort(texts, index, left, j);
            }
            if (i < right)
            {
                indexQuickSort(texts, index, i, right);
            }
        }
    }


}
