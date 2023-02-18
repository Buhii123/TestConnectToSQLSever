using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Mechanical;
using System.Collections.ObjectModel;
using System.Data.Common;
using TestFilterDuctPipeWithSQL.Model;
using System.Windows.Forms;

namespace TestFilterDuctPipeWithSQL
{
    /// <summary>
    /// Interaction logic for ViewMainApp.xaml
    /// </summary>

    public partial class ViewMainApp : Window
    {
        UIDocument uidoc;
        Document doc;

        ObservableCollection<Pipe> Pipes;
        ObservableCollection<Duct> Ducts;
        ObservableCollection<FamilyInstance> PipeFitting;
        ObservableCollection<FamilyInstance> PipeAccessories;
        ObservableCollection<FamilyInstance> DuctFitting;


        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-MTQUN8L\\BUHII;Initial Catalog=MEP;Integrated Security=True");

        DataTable dataTablePipe = new DataTable("MEP_Pipe");
        DataTable dataTableDuct = new DataTable("MEP_Duct");
        DataTable dataTablePipeFitting = new DataTable("MEP_PipeFiting");
        DataTable dataTablePipeAccessorie = new DataTable("MEP_PipeAccessories");
        DataTable dataTableDuctFitting = new DataTable("MEP_DuctFiting");


        SqlDataAdapter adapterCommandPipe = new SqlDataAdapter();
        SqlDataAdapter adapterCommandDuct = new SqlDataAdapter();
        SqlDataAdapter adapterCommandPipeFitting = new SqlDataAdapter();
        SqlDataAdapter adapterCommandPipeAccessorie = new SqlDataAdapter();
        SqlDataAdapter adapterCommandDuctFitting = new SqlDataAdapter();


        DataSet dataSet = new DataSet();

        public ViewMainApp(UIDocument U)
        {
            uidoc = U;
            doc = U.Document;

            List<Pipe> pipes = new FilteredElementCollector(doc)
                .OfClass(typeof(Pipe))
                .OfCategory(BuiltInCategory.OST_PipeCurves)
                .WhereElementIsNotElementType()
                .Cast<Pipe>()
                .ToList();

            List<Duct> ducts = new FilteredElementCollector(doc)
               .OfClass(typeof(Duct))
               .OfCategory(BuiltInCategory.OST_DuctCurves)
               .WhereElementIsNotElementType()
               .Cast<Duct>()
               .Where(d => d.DuctType.FamilyName.Equals("Rectangular Duct"))
               .ToList();

            List<FamilyInstance> pipeFitting = new FilteredElementCollector(doc)
              .OfClass(typeof(FamilyInstance))
              .OfCategory(BuiltInCategory.OST_PipeFitting)
              .WhereElementIsNotElementType()
              .Cast<FamilyInstance>()
              .ToList();

            List<FamilyInstance> pipeAccessories = new FilteredElementCollector(doc)
              .OfClass(typeof(FamilyInstance))
              .OfCategory(BuiltInCategory.OST_PipeAccessory)
              .WhereElementIsNotElementType()
              .Cast<FamilyInstance>()
              .ToList();

            List<FamilyInstance> ductFitting = new FilteredElementCollector(doc)
             .OfClass(typeof(FamilyInstance))
             .OfCategory(BuiltInCategory.OST_DuctFitting)
             .WhereElementIsNotElementType()
             .Cast<FamilyInstance>()
             .ToList();

            Pipes = new ObservableCollection<Pipe>(pipes);
            Ducts = new ObservableCollection<Duct>(ducts);
            PipeFitting = new ObservableCollection<FamilyInstance>(pipeFitting);
            PipeAccessories = new ObservableCollection<FamilyInstance>(pipeAccessories);
            DuctFitting = new ObservableCollection<FamilyInstance>(ductFitting);

            CreateDataPipe.Create(connection, Pipes);
            CreateDataDuct.Create(connection, Ducts);
            CreateDataPipeFitting.Create(connection, PipeFitting);
            CreatePipeAccessories.Create(connection, PipeAccessories);
            CreateDuctFitting.Create(connection, DuctFitting);


            InittializeData();
            InitializeComponent();
        }

        private void InittializeData()
        {
            connection.Open();

            //Pipe
            adapterCommandPipe.TableMappings.Add("Table1", "MEP_Pipe");
            adapterCommandPipe.SelectCommand = new SqlCommand(@"SELECT * FROM MEP_Pipe", connection);
            adapterCommandPipe.DeleteCommand = new SqlCommand(@"DELETE FROM  MEP_Pipe WHERE STT = @STT", connection);

            var dldtPipe1 = adapterCommandPipe.DeleteCommand.Parameters.Add(new SqlParameter("@STT", SqlDbType.Int));
            dldtPipe1.SourceColumn = "STT";
            dldtPipe1.SourceVersion = DataRowVersion.Original;

            adapterCommandPipe.UpdateCommand = new SqlCommand("UPDATE MEP_Pipe SET Comments=@Comments WHERE STT = @STT", connection);
            adapterCommandPipe.UpdateCommand.Parameters.Add("@Comments", SqlDbType.NVarChar, 255, "Comments");
            var dldtPipe2 = adapterCommandPipe.UpdateCommand.Parameters.Add(new SqlParameter("@STT", SqlDbType.Int) { SourceColumn = "STT" });
            dldtPipe2.SourceVersion = DataRowVersion.Original;

            //Duct
            adapterCommandDuct.TableMappings.Add("Table2", "MEP_Duct");
            adapterCommandDuct.SelectCommand = new SqlCommand(@"SELECT * FROM MEP_Duct", connection);

            adapterCommandDuct.DeleteCommand = new SqlCommand(@"DELETE FROM MEP_Duct WHERE STT = @STT", connection);
            var dldtDuct1 = adapterCommandDuct.DeleteCommand.Parameters.Add(new SqlParameter("@STT", SqlDbType.Int));
            dldtDuct1.SourceColumn = "STT";
            dldtDuct1.SourceVersion = DataRowVersion.Original;

            adapterCommandDuct.UpdateCommand = new SqlCommand(@"UPDATE MEP_Duct SET Comments=@Comments WHERE STT = @STT", connection);
            adapterCommandDuct.UpdateCommand.Parameters.Add("@Comments", SqlDbType.NVarChar, 255, "Comments");
            var dldtDuct2 = adapterCommandDuct.UpdateCommand.Parameters.Add(new SqlParameter("@STT", SqlDbType.Int) { SourceColumn = "STT" });
            dldtDuct2.SourceVersion = DataRowVersion.Original;



            //PipeFitting
            adapterCommandPipeFitting.TableMappings.Add("Table3", "MEP_PipeFiting");
            adapterCommandPipeFitting.SelectCommand = new SqlCommand(@"SELECT * FROM MEP_PipeFiting", connection);
            adapterCommandPipeFitting.DeleteCommand = new SqlCommand(@"DELETE FROM MEP_PipeFiting WHERE STT = @STT", connection);

            var dtdtPipeF1 = adapterCommandPipeFitting.DeleteCommand.Parameters.Add(new SqlParameter("@STT", SqlDbType.Int));
            dtdtPipeF1.SourceColumn = "STT";
            dtdtPipeF1.SourceVersion = DataRowVersion.Original;

            adapterCommandPipeFitting.UpdateCommand = new SqlCommand(@"UPDATE MEP_PipeFiting SET Comments=@Comments WHERE STT = @STT", connection);
            adapterCommandPipeFitting.UpdateCommand.Parameters.Add("@Comments", SqlDbType.NVarChar, 255, "Comments");
            var dtdtPipeF2 = adapterCommandPipeFitting.UpdateCommand.Parameters.Add(new SqlParameter("@STT", SqlDbType.Int) { SourceColumn = "STT" });
            dtdtPipeF2.SourceVersion = DataRowVersion.Original;


            //PipeAccessorie
            adapterCommandPipeAccessorie.TableMappings.Add("Table4", "MEP_PipeAccessories");
            adapterCommandPipeAccessorie.SelectCommand = new SqlCommand(@"SELECT * FROM MEP_PipeAccessories", connection);
            adapterCommandPipeAccessorie.DeleteCommand = new SqlCommand(@"DELETE FROM MEP_PipeAccessories WHERE STT = @STT", connection);

            var dldtPipeA1 = adapterCommandPipeAccessorie.DeleteCommand.Parameters.Add(new SqlParameter("@STT", SqlDbType.Int));
            dldtPipeA1.SourceColumn = "STT";
            dldtPipeA1.SourceVersion = DataRowVersion.Original;

            adapterCommandPipeAccessorie.UpdateCommand = new SqlCommand(@"UPDATE MEP_PipeAccessories SET Comments=@Comments WHERE STT = @STT", connection);
            adapterCommandPipeAccessorie.UpdateCommand.Parameters.Add("@Comments", SqlDbType.NVarChar, 255, "Comments");
            var dldtPipeA2 = adapterCommandPipeAccessorie.UpdateCommand.Parameters.Add(new SqlParameter("@STT", SqlDbType.Int) { SourceColumn = "STT" });
            dldtPipeA2.SourceVersion = DataRowVersion.Original;


            //DuctFitting
            adapterCommandDuctFitting.TableMappings.Add("Table5", "MEP_DuctFiting");
            adapterCommandDuctFitting.SelectCommand = new SqlCommand(@"SELECT * FROM MEP_DuctFiting", connection);
            adapterCommandDuctFitting.DeleteCommand = new SqlCommand(@"DELETE FROM MEP_DuctFiting WHERE STT = @STT", connection);

            var dldtDuctF1 = adapterCommandDuctFitting.DeleteCommand.Parameters.Add(new SqlParameter("@STT", SqlDbType.Int));
            dldtDuctF1.SourceColumn = "STT";
            dldtDuctF1.SourceVersion = DataRowVersion.Original;

            adapterCommandDuctFitting.UpdateCommand = new SqlCommand(@"UPDATE MEP_DuctFiting SET Comments=@Comments WHERE STT = @STT", connection);
            adapterCommandDuctFitting.UpdateCommand.Parameters.Add("@Comments", SqlDbType.NVarChar, 255, "Comments");
            var dldtDuctF2 = adapterCommandDuctFitting.UpdateCommand.Parameters.Add(new SqlParameter("@STT", SqlDbType.Int) { SourceColumn = "STT" });
            dldtDuctF2.SourceVersion = DataRowVersion.Original;




            dataSet.Tables.Add(dataTablePipe);
            dataSet.Tables.Add(dataTableDuct);
            dataSet.Tables.Add(dataTablePipeFitting);
            dataSet.Tables.Add(dataTablePipeAccessorie);
            dataSet.Tables.Add(dataTableDuctFitting);



        }



        private void LoadDataTable()
        {
            dataTablePipe.Rows.Clear();
            dataTableDuct.Rows.Clear();
            dataTablePipeFitting.Rows.Clear();
            dataTablePipeAccessorie.Rows.Clear();
            dataTableDuctFitting.Rows.Clear();

            adapterCommandPipe.Fill(dataSet, "Table1");
            adapterCommandDuct.Fill(dataSet, "Table2");
            adapterCommandPipeFitting.Fill(dataSet, "Table3");
            adapterCommandPipeAccessorie.Fill(dataSet, "Table4");
            adapterCommandDuctFitting.Fill(dataSet, "Table5");
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataTable();
            dataPipe.DataContext = dataTablePipe.DefaultView;
            dataDuct.DataContext = dataTableDuct.DefaultView;
            dataPipeFitting.DataContext = dataTablePipeFitting.DefaultView;
            dataPipeAcc.DataContext = dataTablePipeAccessorie.DefaultView;
            dataDuctFitting.DataContext = dataTableDuctFitting.DefaultView;
        }

        private void Button_Load(object sender, RoutedEventArgs e)
        {
            RevitUpdate.Update(doc, "MEP_Duct");
            RevitUpdate.Update(doc, "MEP_DuctFiting");
            RevitUpdate.Update(doc, "MEP_Pipe");
            RevitUpdate.Update(doc, "MEP_PipeAccessories");
            RevitUpdate.Update(doc, "MEP_PipeFiting");



        }

        private void Button_Update(object sender, RoutedEventArgs e)
        {

            adapterCommandPipe.Update(dataSet, "Table1");
            dataTablePipe.Rows.Clear();
            adapterCommandPipe.Fill(dataSet, "Table1");


            adapterCommandDuct.Update(dataSet, "Table2");
            dataTableDuct.Rows.Clear();
            adapterCommandDuct.Fill(dataSet, "Table2");

            adapterCommandPipeFitting.Update(dataSet, "Table3");
            dataTablePipeFitting.Rows.Clear();
            adapterCommandPipeFitting.Fill(dataSet, "Table3");

            adapterCommandPipeAccessorie.Update(dataSet, "Table4");
            dataTablePipeAccessorie.Rows.Clear();
            adapterCommandPipeAccessorie.Fill(dataSet, "Table4");

            adapterCommandDuctFitting.Update(dataSet, "Table5");
            dataTableDuctFitting.Rows.Clear();
            adapterCommandDuctFitting.Fill(dataSet, "Table5");



        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            using (Transaction tran = new Transaction(doc, "Delele"))
            {
                tran.Start();
                DataRowView selectedItem1 = (DataRowView)dataPipe.SelectedItem;

                if (selectedItem1 != null)
                {
                    doc.Delete(new ElementId(int.Parse(selectedItem1.Row["ElementID"].ToString())));
                    selectedItem1.Delete();
                    adapterCommandPipe.Update(dataSet, "Table1");

                }
                DataRowView selectedItem2 = (DataRowView)dataDuct.SelectedItem;
                if (selectedItem2 != null)
                {
                    doc.Delete(new ElementId(int.Parse(selectedItem2.Row["ElementID"].ToString())));
                    selectedItem2.Delete();
                    adapterCommandDuct.Update(dataSet, "Table2");



                }
                DataRowView selectedItem3 = (DataRowView)dataPipeFitting.SelectedItem;
                if (selectedItem3 != null)
                {
                    doc.Delete(new ElementId(int.Parse(selectedItem3.Row["ElementID"].ToString())));
                    selectedItem3.Delete();
                    adapterCommandPipeFitting.Update(dataSet, "Table3");



                }
                DataRowView selectedItem4 = (DataRowView)dataPipeAcc.SelectedItem;
                if (selectedItem4 != null)
                {
                    doc.Delete(new ElementId(int.Parse(selectedItem4.Row["ElementID"].ToString())));
                    selectedItem4.Delete();
                    adapterCommandPipeAccessorie.Update(dataSet, "Table4");



                }
                DataRowView selectedItem5 = (DataRowView)dataDuctFitting.SelectedItem;
                if (selectedItem5 != null)
                {
                    doc.Delete(new ElementId(int.Parse(selectedItem5.Row["ElementID"].ToString())));
                    selectedItem5.Delete();
                    adapterCommandDuctFitting.Update(dataSet, "Table5");



                }
                tran.Commit();
            }

        }

    }
}
