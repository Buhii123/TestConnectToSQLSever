
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFilterDuctPipeWithSQL.Model
{
    public static class RevitUpdate
    {
        public static void Update(Document doc,string name)
        {
            var connection = new SqlConnection("Data Source=DESKTOP-MTQUN8L\\BUHII;Initial Catalog=MEP;Integrated Security=True");
            connection.Open();

            using (var commandPipe = new SqlCommand("Select * From ["+ name +"]", connection))
            {
                using (TransactionGroup TranGruop = new TransactionGroup(doc))
                {
                    TranGruop.Start();
                    var reader = commandPipe.ExecuteReader();

                    while (reader.Read())
                    {
                        try 
                        {
                            var elementID = reader["ElementID"].ToString();
                            var comments = reader["Comments"].ToString();
                            ElementId elementId = new ElementId(int.Parse(elementID));
                            Element el = doc.GetElement(elementId);
                            using (Transaction tran = new Transaction(doc, "SQLComment"))
                            {
                                tran.Start();
                                el.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set(comments);
                                tran.Commit();
                            }
                        }
                        catch 
                        {
                            continue;
                        }
                        
                    }
                    TranGruop.Commit();
                }
            }
            connection.Close();

        }
    }
}
