using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WebBasePM
{
    public enum DocumentType
    {
        PM, Install
    }
    public class ReadInformation
    {
        public string fontFamily = "Arial"; //"Tahoma";
        public int fontSize = 10;
        public int fontColor = 0x000000;
        public int fontBold = 0;
        public int fontItalic = 0;
        public DocumentType DocType;

        public void setDefault(string family, int size, int color, int bold, int italic)
        {
            fontSize = size;
            fontColor = color;
            fontFamily = family;
            fontBold = bold;
            fontItalic = italic;
        }

        protected string getCenter(string text, int pass, char ch)
        {
            string tmp = text.Trim();
            int i, j;
            for (i = pass; i < tmp.Length && tmp[i] == ch; i++) ;
            for (j = tmp.Length - pass - 1; j > 0 && tmp[j] == ch; j--) ;
            return tmp.Substring(i, j - i + 1);     //fixed
        }

        public string rearrangeNumber(string text)
        {
            int tmpInt;
            if (int.TryParse(text, out tmpInt))
            {
                return tmpInt.ToString();
            }
            return text;
        }

        //////////////////////////////////////////////////////////////////////////////

        public tableListWord readOutputTable(TextReader reader, SetOfTableList tables)
        {
            tableListWord tableTmp = null;
            string line = null;
            string[] strtmps = null;
            int col;

            if ((line = reader.ReadLine()) != null)
            {
                tableTmp = new tableListWord(line);
                if ((line = reader.ReadLine()) != null)
                {
                    strtmps = line.Split(new char[] { '\t' });
                    col = strtmps.Length;
                    tableTmp.addHeader(strtmps);
                    if (((line = reader.ReadLine()) != null))
                    {
                        tableTmp.addColWidth(line.Split(new char[] { '\t' }));
                    }
                    while ((line = reader.ReadLine()) != null)
                    {
                        object[] objtmps = new object[col];
                        int i = 0, j = 0;
                        bool flag = true;
                        /* read first column */
                        while (i < line.Length && flag)
                        {
                            if (line[i] == '\t')
                            {
                                objtmps[0] = parseSpecialSymbol(line.Substring(0, i));
                                flag = false;
                            }
                            i++;
                        }
                        j = i;
                        // read remained column (may be reconsider about ';')
                        // next version should use readFormat since 1st col
                        int c = 1;
                        while (i < line.Length)
                        {
                            if (line[i] == ';' && (i == line.Length - 1 || line[i + 1] == '\t'))
                            { // end column? && last column || there are more column
                                if (c < col)
                                {
                                    objtmps[c] = readFormat(line.Substring(j, i - j), tables); //fixed
                                }
                                c++;
                                i++;
                                j = i + 1;
                            }
                            i++;
                        }
                        tableTmp.addRow(objtmps);

                    }
                }
            }
            return tableTmp;
        }

        public object readFormat(string f, SetOfTableList t) //java ver return string
        {
            ArrayList tmps = new ArrayList();
            //String [] strtmps;
            char[] chs = new char[1024];
            int pos = 0;
            int i = 0;
            bool flag = true;

            if (f.Length < 2 || f[0] != '\"')
            {
                return null;
            }
            // read formated text in double quote
            for (i = 1; i < f.Length && flag; i++)
            {
                if (f[i] == '\"')
                {
                    chs[pos] = '\0';
                    flag = false;
                }
                else if (f[i] == '\\')
                {
                    if (i + 1 >= f.Length)
                    {
                        return null;
                    }
                    else if (f[i + 1] == '\\')
                    {
                        chs[pos] = '\\';
                        pos++;
                        i++;
                    }
                    else if (f[i + 1] == '\"')
                    {
                        chs[pos] = '\"';
                        pos++;
                        i++;
                    }
                    else if (f[i + 1] == 'n')
                    {
                        chs[pos] = '\n';
                        pos++;
                        i++;
                    }
                    else if (f[i + 1] == 't')
                    {
                        chs[pos] = '\t';
                        pos++;
                        i++;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    chs[pos] = f[i];
                    pos++;
                }
                //System.out.println(chs);
            }
            if (i >= f.Length && flag)
            { /* end with non '\"' */
                return null;
            }

            string ans = new string(chs, 0, pos);
            //System.out.println(">>>" + ans);
            tmps.Add(ans);

            // read formated variables behind double quote
            pos = 0;
            while (i < f.Length)
            {
                if (f[i] == ',')
                {
                    chs[pos] = '\0';
                    ans = new string(chs, 0, pos);
                    //System.out.println(">>" + ans);
                    tmps.Add(ans);
                    pos = 0;
                }
                else if (f[i] == '\\')
                {
                    if (i + 1 >= f.Length)
                    {
                        return null;
                    }
                    else if (f[i + 1] == '\\')
                    {
                        chs[pos] = '\\';
                        pos++;
                        i++;
                    }
                    else if (f[i + 1] == ',')
                    {
                        chs[pos] = ',';
                        pos++;
                        i++;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    chs[pos] = f[i];
                    pos++;
                }
                //System.out.println(chs);
                i++;
            }
            chs[pos] = '\0';
            ans = new string(chs, 0, pos);
            //System.out.println(">>" + ans);
            tmps.Add(ans);

            /* scan format */
            // This part need to be normalized
            string tmp = (string)tmps[0];
            ans = "";
            pos = 1;
            for (i = 0; i < tmp.Length; i++)
            {
                if (tmp[i] == '%')
                {
                    string s;
                    if (i + 1 >= tmp.Length)
                    {
                        return null;
                    }
                    else if (tmp[i + 1] == '%')
                    {
                        ans += "%";
                        i++;
                    }
                    /// %s (search string) tableName@key@outputColumn
                    /// %c (search column) tableName@outputColumn
                    /// %C
                    /// %e (select enumerate) tableName@keyColumn@element@value
                    /// %E (select enumerate) tableName@keyIndex@key@outputColumn@element@value
                    /// %p (search by prefix) tableName@key@outputColumn
                    /// %x (check existent table) tableName@outputSequence
                    /// %X (check existent key in field) tableName@key@Column@outputSequence
                    else if (tmp[i + 1] == 's' || tmp[i + 1] == 'c' || tmp[i + 1] == 'C' || tmp[i + 1] == 'e' || tmp[i + 1] == 'E' ||
                             tmp[i + 1] == 'p' || tmp[i + 1] == 'x' || tmp[i + 1] == 'X')
                    {
                        if ((s = (string)translate((string)tmps[pos], tmp[i + 1].ToString(), t)) == null)
                        {
                            s = "";
                        }
                        ans += s;
                        pos++;
                        i++;
                    }
                    /// %T (show table) tableName@columnSequence@headerSequence <<<<< if option is 'T', it'll immediately return tableListWord
                    else if (tmp[i + 1] == 'T')
                    {
                        tableListWord tlw;
                        if ((tlw = (tableListWord)translate((string)tmps[pos], "T", t)) != null)
                        {
                            return tlw;
                        }
                        pos++;
                        i++;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    ans += tmp[i];
                }
            }

            return ans;
        }

        public object translate(string text, string mode, SetOfTableList tables)
        { /* please check undefined case */
            //String ans = "";

            /* mode case */
            if (mode.Equals("s") || mode.Equals("p"))
            {
                /* get table name */
                int i = 0;
                int k = 3;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                if (i == text.Length)
                {
                    return null;
                }
                string tableName = text.Substring(0, i - 1);
                /* get key */
                int j = i;
                k = 1;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                string key = text.Substring(j, i - j - 1);  //fixed

                /* get indexField */
                /*j = i;
                k = 1;
                while (i < text.length() && k != 0) {
                    if (text.charAt(i) == '@') {
                        k--;
                    }
                    i++;
                }*/
                int indexField = int.Parse(text.Substring(i));  /// may use tryParse

                tableList table = tables.getTableList(tableName);
                if (table != null)
                {
                    if (mode.Equals("s"))
                    {
                        return table.searchField(key, indexField);
                    }
                    else if (mode.Equals("p"))
                    {
                        return table.searchPrefixField(key, indexField);
                    }
                }
            }
            else if (mode.Equals("T"))  //////////////////////////////////////////////////////////////////////////
            {
                /* get table name */
                int i = 0;
                int k = 3;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                if (i == text.Length)
                {
                    return null;
                }
                string tableName = text.Substring(0, i - 1);
                /* get a sequence of column */
                int j = i;
                k = 1;
                int c = 0;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    else if (text[i] == '-')
                    {
                        c++;
                    }
                    i++;
                }
                int[] col = new int[c + 1];
                string[] header = new string[c + 1];
                string[] colWidth = null;
                string strtmp = text.Substring(j, i - j - 1);   //fixed
                c = 0;
                k = 0;
                for (int m = 0; m < strtmp.Length; m++)
                {
                    if (strtmp[m] == '-')
                    {
                        col[c] = int.Parse(strtmp.Substring(k, m - k));     /// may use tryParse fixed//fixed
                        c++;
                        k = m + 1;
                    }
                }
                col[c] = int.Parse(strtmp.Substring(k));    /// may use tryParse

                /* get header */
                ///////////////////////////////////
                j = i;
                k = 1;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                if (k == 0) // add column width
                {
                    strtmp = text.Substring(j, i - j - 1);

                    colWidth = new string[header.Length];
                    c = 0;
                    k = 0;
                    string strtmp2 = text.Substring(i);
                    for (int m = 0; m < strtmp2.Length; m++)
                    {
                        if (strtmp2[m] == '-')
                        {
                            colWidth[c] = strtmp2.Substring(k, m - k);     //fixed
                            c++;
                            k = m + 1;
                        }
                    }
                    colWidth[c] = strtmp2.Substring(k);
                }
                else
                {
                    strtmp = text.Substring(j);
                }

                ///////////////////////////////////
                c = 0;
                k = 0;
                //strtmp = text.Substring(i);
                for (int m = 0; m < strtmp.Length; m++)
                {
                    if (strtmp[m] == '-')
                    {
                        header[c] = strtmp.Substring(k, m - k);     //fixed
                        c++;
                        k = m + 1;
                    }
                }
                header[c] = strtmp.Substring(k);

                tableList table = tables.getTableList(tableName);
                if (table != null)
                {
                    tableListWord tlwtmp = new tableListWord(table, col);   //// may be also add border
                    tlwtmp.addHeader(header);
                    if (colWidth != null)
                    {
                        tlwtmp.addColWidth(colWidth);
                    }
                    return tlwtmp;       ////////////////////////////////////// change
                }
            }
            else if (mode.Equals("e"))
            {
                /* get table name */
                int i = 0;
                int k = 3;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                if (i == text.Length)
                {
                    return null;
                }
                string tableName = text.Substring(0, i - 1);

                /* get indexField */
                int j = i;
                k = 1;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                //String acol = text.substring(j, i-1);
                int indexField = int.Parse(text.Substring(j, i - j - 1));   /// may use tryParse//fixed

                /* get a sequence of element */
                j = i;
                k = 1;
                int c = 0;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '|')
                    {
                        if (i + 1 >= text.Length)
                        {
                            return null;
                        }
                        else if (text[i + 1] == '|')
                        {
                            i++;
                        }
                        else if (text[i + 1] == '-')
                        {
                            i++;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (text[i] == '@')
                    {
                        k--;
                    }
                    else if (text[i] == '-')
                    {
                        c++;
                    }
                    i++;
                }
                string[] element = new string[c + 1];
                string[] value = new string[c + 2];
                string strtmp = text.Substring(j, i - j - 1);   //fixed
                for (int z = 0; z <= c; z++)
                {
                    element[z] = "";
                    value[z] = "";
                }
                value[c + 1] = "";

                c = 0;
                k = 0;
                for (int m = 0; m < strtmp.Length; m++)
                {
                    if (strtmp[m] == '|')
                    {
                        if (m + 1 >= strtmp.Length)
                        {
                            return null;
                        }
                        else if (strtmp[m + 1] == '|')
                        {
                            element[c] += strtmp.Substring(k, m - k) + "|";
                            m++;
                            k = m + 1;
                        }
                        else if (strtmp[m + 1] == '-')
                        {
                            element[c] += strtmp.Substring(k, m - k) + "-";
                            m++;
                            k = m + 1;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (strtmp[m] == '-')
                    {
                        element[c] += strtmp.Substring(k, m - k); //fixed
                        c++;
                        k = m + 1;
                    }
                }
                element[c] += strtmp.Substring(k);

                /* get a sequence of value */
                c = 0;
                k = 0;
                strtmp = text.Substring(i);
                for (int m = 0; m < strtmp.Length; m++)
                {
                    if (strtmp[m] == '|')
                    {
                        if (m + 1 >= strtmp.Length)
                        {
                            return null;
                        }
                        else if (strtmp[m + 1] == '|')
                        {
                            value[c] += strtmp.Substring(k, m - k) + "|";
                            m++;
                            k = m + 1;
                        }
                        else if (strtmp[m + 1] == '-')
                        {
                            value[c] += strtmp.Substring(k, m - k) + "-";
                            m++;
                            k = m + 1;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (strtmp[m] == '-')
                    {
                        value[c] += strtmp.Substring(k, m - k);  //fixed
                        c++;
                        k = m + 1;
                    }
                }
                if (c == value.Length - 1)
                { // there is else case
                    value[c] += strtmp.Substring(k);
                }
                else if (c == value.Length - 2)
                { // non else case
                    value[c] += strtmp.Substring(k);
                    value[c + 1] = "";
                }

                tableList table = tables.getTableList(tableName);
                if (table != null)
                {
                    object[] tmps = table.getRow(0);
                    if (indexField < tmps.Length && tmps[indexField] is string)
                    {
                        for (j = 0; j < element.Length; j++)
                        {
                            if (((string)tmps[indexField]).Equals(element[j]))
                            {
                                return value[j];
                            }
                        }
                        return value[j];
                    }
                }

                return null;
            }
            else if (mode.Equals("E"))
            {
                /* get table name */
                int i = 0;
                int k = 3;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                if (i == text.Length)
                {
                    return null;
                }
                string tableName = text.Substring(0, i - 1);

                /* get key indexField */
                int j = i;
                k = 1;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                //String acol = text.substring(j, i-1);
                int keyIndexField = int.Parse(text.Substring(j, i - j - 1));   /// may use tryParse//fixed

                /* get key */
                j = i;
                k = 1;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                string key = text.Substring(j, i - j - 1);  //fixed

                /* get indexField */
                j = i;
                k = 1;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                int indexField = int.Parse(text.Substring(j, i - j - 1));   /// may use tryParse//fixed

                /* get a sequence of element */
                j = i;
                k = 1;
                int c = 0;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '|')
                    {
                        if (i + 1 >= text.Length)
                        {
                            return null;
                        }
                        else if (text[i + 1] == '|')
                        {
                            i++;
                        }
                        else if (text[i + 1] == '-')
                        {
                            i++;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (text[i] == '@')
                    {
                        k--;
                    }
                    else if (text[i] == '-')
                    {
                        c++;
                    }
                    i++;
                }
                string[] element = new string[c + 1];
                string[] value = new string[c + 3];
                string strtmp = text.Substring(j, i - j - 1);   //fixed
                for (int z = 0; z <= c; z++)
                {
                    element[z] = "";
                    value[z] = "";
                }
                value[c + 1] = "";

                c = 0;
                k = 0;
                for (int m = 0; m < strtmp.Length; m++)
                {
                    if (strtmp[m] == '|')
                    {
                        if (m + 1 >= strtmp.Length)
                        {
                            return null;
                        }
                        else if (strtmp[m + 1] == '|')
                        {
                            element[c] += strtmp.Substring(k, m - k) + "|";
                            m++;
                            k = m + 1;
                        }
                        else if (strtmp[m + 1] == '-')
                        {
                            element[c] += strtmp.Substring(k, m - k) + "-";
                            m++;
                            k = m + 1;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (strtmp[m] == '-')
                    {
                        element[c] += strtmp.Substring(k, m - k); //fixed
                        c++;
                        k = m + 1;
                    }
                }
                element[c] += strtmp.Substring(k);

                /* get a sequence of value */
                c = 0;
                k = 0;
                strtmp = text.Substring(i);
                for (int m = 0; m < strtmp.Length; m++)
                {
                    if (strtmp[m] == '|')
                    {
                        if (m + 1 >= strtmp.Length)
                        {
                            return null;
                        }
                        else if (strtmp[m + 1] == '|')
                        {
                            value[c] += strtmp.Substring(k, m - k) + "|";
                            m++;
                            k = m + 1;
                        }
                        else if (strtmp[m + 1] == '-')
                        {
                            value[c] += strtmp.Substring(k, m - k) + "-";
                            m++;
                            k = m + 1;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (strtmp[m] == '-')
                    {
                        value[c] += strtmp.Substring(k, m - k);  //fixed
                        c++;
                        k = m + 1;
                    }
                }
                if (c == value.Length - 1)
                { // there is else case
                    value[c] += strtmp.Substring(k);
                }
                else if (c == value.Length - 2)
                { // non else case
                    value[c] += strtmp.Substring(k);
                    value[c + 1] = "";
                }
                else if (c == value.Length - 3)
                { // non else case
                    value[c] += strtmp.Substring(k);
                    value[c + 1] = "";
                    value[c + 2] = "";
                }

                tableList table = tables.getTableList(tableName);
                if (table != null)
                {
                    strtmp = (string)table.searchField(keyIndexField, key, indexField);
                    if (strtmp != null)
                    {
                        for (j = 0; j < element.Length; j++)
                        {
                            if (strtmp.Equals(element[j]))
                            {
                                return value[j];
                            }
                        }
                        return value[j];
                    }
                    else
                    {
                        return value[value.Length - 1];
                    }
                }

                return null;
            }
            else if (mode.Equals("c"))
            {
                /* get table name */
                int i = 0;
                int k = 3;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                if (i == text.Length)
                {
                    return null;
                }
                string tableName = text.Substring(0, i - 1);
                /* get indexField */
                int indexField = int.Parse(text.Substring(i));      /// may use tryParse
                tableList table = tables.getTableList(tableName);
                if (table != null)
                {
                    object[] tmps = table.getRow(0);
                    if (indexField < tmps.Length && tmps[indexField] is string)
                    {
                        return (string)tmps[indexField];
                    }
                }
            }
            else if (mode.Equals("x"))
            {
                /* get table name */
                int i = 0;
                int k = 3;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                if (i == text.Length)
                {
                    return null;
                }
                string tableName = text.Substring(0, i - 1);
                /* get a sequence of output */
                string[] output = new string[2];
                string strtmp = text.Substring(i);
                int c = 0;
                k = 0;
                for (int m = 0; m < strtmp.Length; m++)
                {
                    if (strtmp[m] == '-')
                    {
                        output[c] = strtmp.Substring(k, m - k); //fixed
                        c++;
                        k = m + 1;
                    }
                }
                output[c] = strtmp.Substring(k);

                tableList table = tables.getTableList(tableName);
                if (table == null)
                {
                    return output[0];
                }
                else
                {
                    return output[1];
                }
            }
            else if (mode.Equals("X"))
            {
                /* get table name */
                int i = 0;
                int k = 3;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                if (i == text.Length)
                {
                    return null;
                }
                string tableName = text.Substring(0, i - 1);
                /* get key */
                int j = i;
                k = 1;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                string key = text.Substring(j, i - j - 1);  //fixed

                /* get indexField */
                j = i;
                k = 1;
                while (i < text.Length && k != 0)
                {
                    if (text[i] == '@')
                    {
                        k--;
                    }
                    i++;
                }
                int indexField = int.Parse(text.Substring(j, i - j - 1));   /// may use tryParse//fixed

                /* get a sequence of output */
                string[] output = new string[2];
                string strtmp = text.Substring(i);
                int c = 0;
                k = 0;
                for (int m = 0; m < strtmp.Length; m++)
                {
                    if (strtmp[m] == '-')
                    {
                        output[c] = strtmp.Substring(k, m - k); //fixed
                        c++;
                        k = m + 1;
                    }
                }
                output[c] = strtmp.Substring(k);

                tableList table = tables.getTableList(tableName);
                if (table != null)
                {
                    object tmp = table.searchField(indexField, key, indexField);
                    if (tmp != null && tmp is string && key.Equals((string)tmp))
                    {
                        return output[1];
                    }
                }
                return output[0];
            }

            return null;
        }

        public string parseSpecialSymbol(string text)
        {
            string[] splits = text.Split(new string[] { "\\n" }, StringSplitOptions.None);
            string join = string.Join("\n", splits);

            splits = join.Split(new string[] { "\\t" }, StringSplitOptions.None);
            join = string.Join("\t", splits);

            return join;
        }
    }
}
