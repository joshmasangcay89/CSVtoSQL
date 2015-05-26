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
        static void Main()
        {
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
            }
            catch (Exception e)
            {
                Console.WriteLine(">>>" + e.Message);
                Console.ReadLine();
            }

            GetJTRecords();            
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

                //Context.JT_Temp.Add(jt_temp);
                //Context.SaveChanges();      
                //GetJTRecords();
            }
        }

        public static void GetJTRecords()
        {
            using (var Context = new AH_ODS_DBEntities())
            {
                //Goal:
                //SELECT EnvelopeCode, Branch_COA, AQ_COA, AQ_Branch, SUM(Amount), AQ_PostStatus FROM JT_Temp
                //GROUP BY EnvelopeCode, Branch_COA, AQ_COA, AQ_Branch, AQ_PostStatus

                //var csvFilteredRecord = Context.JT_Temp.SqlQuery("SELECT * FROM JT_Temp").ToList<JT_Temp>(); 
                // GROUP BY -- No go; Manual SELECT -- No go;
                
                try
                {


                    #region StackOverflow
                    //var csvFilteredRecord = (
                    //    from c in Context.JT_Temp
                    //    group c by new
                    //    {
                    //        c.EnvelopeCode,
                    //        c.Branch_COA,
                    //        c.AQ_COA,
                    //        c.AQ_Branch,
                    //        c.AQ_PostStatus
                    //    } into i
                    //    select new JT_Temp
                    //    {
                    //        EnvelopeCode = i.Key.EnvelopeCode,
                    //        Branch_COA = i.Key.Branch_COA,
                    //        AQ_COA = i.Key.AQ_COA,
                    //        AQ_Branch = i.Key.AQ_Branch,
                    //        //TO-DO SUM(Amount),
                    //        AQ_PostStatus = i.Key.AQ_PostStatus
                    //    }).ToList(); 
                    #endregion
                    #region Group test Linq to Entity
                    //var csvFilteredRecord = Context.JT_Temp
                    //.GroupBy(g => new
                    //{
                    //    grp_env = g.EnvelopeCode,
                    //    grp_branCOA = g.Branch_COA,
                    //    grp_aqcoa = g.AQ_COA,
                    //    grp_aqbranch = g.AQ_Branch,
                    //    //grp_amount = g.Amount,
                    //    grp_poststat = g.AQ_PostStatus
                    //})
                    //.Select(s => new JT_Temp 
                    //{ 
                    //    EnvelopeCode = s.Key.grp_env,
                    //    Branch_COA = s.Key.grp_branCOA,
                    //    AQ_COA = s.Key.grp_aqcoa,
                    //    AQ_Branch = s.Key.grp_aqbranch,
                    //    //SUM(Amount)
                    //    AQ_PostStatus = s.Key.grp_poststat
                    //})
                    //.ToList(); 
                    #endregion
                    #region MyRegion
                    //var csvFilteredRecord = (
                    //            from c in Context.JT_Temp   
                    //            group i by new
                    //            {
                    //                c.EnvelopeCode,
                    //                c.Branch_COA,
                    //                c.AQ_COA,
                    //                c.AQ_Branch,
                    //                c.AQ_PostStatus
                    //            } into i

                    //            select new JT_Temp
                    //            {
                    //                EnvelopeCode = i.Key.EnvelopeCode,
                    //                Branch_COA = i.Key.Branch_COA,
                    //                AQ_COA = i.Key.AQ_COA,
                    //                AQ_Branch = i.Key.AQ_Branch,
                    //                //sum(amount)
                    //                AQ_PostStatus = i.Key.AQ_PostStatus
                    //            }).ToList();
                    
                    //.AsEnumerable()
                    //.Select(x => new JT_Temp
                    //{
                    //    EnvelopeCode = x.EnvelopeCode,
                    //    Branch_COA = x.Branch_COA,
                    //    AQ_COA = x.AQ_COA,
                    //    AQ_Branch = x.AQ_Branch,
                    //    //sum(amount)
                    //    AQ_PostStatus = x.AQ_PostStatus
                    //})
                    //.ToList(); 
                    #endregion

                    var csvFilteredRecord = from con in Context.JT_Temp
                                            group con by
                                            con.EnvelopeCode
                                            + con.Branch_COA
                                            + con.AQ_COA
                                            + con.AQ_Branch; ; 
                                            //+ con.AQ_PostStatus;


                    foreach (var Record in csvFilteredRecord)
                    {
                        Console.WriteLine(Record.Key
                                //+ Record.Branch_COA
                                //+ Record.AQ_COA
                                ////+ Record.Amount
                                //+ Record.AQ_PostStatus
                            );
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("---------- " + e.Message);
                    Console.ReadLine();
             
                }
                #region Nope!

                //foreach (var Record in xxx)
                //{
                //    Console.WriteLine(
                //         Record.EnvelopeCode
                //         + Record.Branch_COA
                //         + Record.AQ_COA
                //        //+ Record.Amount
                //         + Record.AQ_PostStatus);
                //}



                ///working but no group by
                //if (csvFilteredRecord.Count >= 1)
                //{
                //    //List<JT_Temp> result;
                //    foreach (var Record in csvFilteredRecord)
                //    {
                //        Console.WriteLine(
                //            Record.EnvelopeCode
                //            + Record.Branch_COA
                //            + Record.AQ_COA
                //            + Record.Amount
                //            + Record.AQ_PostStatus
                //        );
                //    }

                //} 
                #endregion
            }
        }

    }
}
