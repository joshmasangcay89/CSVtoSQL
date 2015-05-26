using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace CSVtoSQL
{
    class Program
    {
        static string Main()
        {
            /// 
            /// CSV Headers: Code PostDate, T, Description, RecTyp, PropAccountNmbr, Amount
            ///

            string[] CSVs = Directory.GetFiles(@"C:\csv\");
            char[] delimiterChars = { ',' };

            try
            {
                foreach (string Files in CSVs)
                {
                    string[] query = File.ReadAllLines(Files);                   
                    string[] lamanngcsvline;
                    int i = 0;
                    foreach (var s in query)
                    {
                        if (i > 0)
                        {
                            lamanngcsvline = s.Split(delimiterChars);
                            if (lamanngcsvline.Count() > 1)
                            {
                                #region lamanngcsvline list
                                //Console.WriteLine(
                                //      lamanngcsvline[0].Trim() 
                                //    + lamanngcsvline[1].Trim() 
                                //    + lamanngcsvline[2].Trim() 
                                //    + lamanngcsvline[3].Trim() 
                                //    + lamanngcsvline[4].Trim() 
                                //    + lamanngcsvline[5].Trim() 
                                //    + lamanngcsvline[6].Trim()); 
                                #endregion                               
                                SaveToSQL(Files, lamanngcsvline, lamanngcsvline.Count(), i);
                            }
                        }
                        i++;
                    }                    
                }
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(">>>" + e.Message);
            }
            return "Done";
        }

        public static void SaveToSQL(string filename_NE, string[] csvData, int csvDataLines, int lineno)
        {
            using (var Context = new AH_ODS_DBEntities())
            {
                JT_Temp jt_temp = new JT_Temp();
                DateTime lastSyncDate = DateTime.UtcNow;
               
                jt_temp.ExtSerial = "temp";
                jt_temp.EnvelopeCode = filename_NE.TrimEnd(new Char[] { '.', 't', 'x' });
                jt_temp.Line = lineno;
                jt_temp.Lines = csvDataLines;
                jt_temp.Branch_COA = "TLV";
                jt_temp.AQ_COA = csvData[5].Trim();
                jt_temp.AQ_Branch = "TLV";
                jt_temp.RecordType = csvData[4].Trim();
                jt_temp.Amount = Convert.ToDouble(csvData[6].Trim());
                jt_temp.PostDate = csvData[1].Trim();
                jt_temp.AQ_PostStatus = 0;
                jt_temp.AQ_StatusDate = lastSyncDate.ToString();
                jt_temp.DTS = lastSyncDate.ToString();
                jt_temp.LineDescription = csvData[3].Trim();

                #region csvData Array List
                //Console.WriteLine( 
                //      csvData[0] //Code
                //    + csvData[1].Trim() //PostDate
                //    + csvData[2].Trim() //T
                //    + csvData[3].Trim() //Description
                //    + csvData[4].Trim() //RecTyp
                //    + csvData[5].Trim() //PropAccountNmbr
                //    + csvData[6].Trim()); //Amount 
                #endregion

                Context.JT_Temp.Add(jt_temp);
                Context.SaveChanges();                
            }
            
        }
    }
}
