using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmAssist.Models;
using SQLite;

namespace PharmAssist.Database
{
    public class PharmAssistDB
    {
        private readonly SQLiteAsyncConnection _connection;

        public PharmAssistDB(string DbPath)
        {
            _connection =new SQLiteAsyncConnection(DbPath);
            _connection.CreateTableAsync<OpMethod>();
            _connection.CreateTableAsync<DoctorsDetailModel>();
            _connection.CreateTableAsync<MedicineModel>();
            _connection.CreateTableAsync<DoctorsPriscriptionModel>();
            _connection.CreateTableAsync<PriscriptionModelDB>();
            _connection.CreateTableAsync<DoctorsPriscriptionModelDB>();
            _connection.CreateTableAsync<DrPriMedicineModel>();
        }

        //OpMethod
        public Task<List<OpMethod>> GetOpTicketList()
        {
            return _connection.Table<OpMethod>().ToListAsync(); 
        }

        public Task<int> AddOpTicket(OpMethod opMethod)
        {
            return _connection.InsertAsync(opMethod);          
        }


        //DrPriMedicine
        public Task<List<DrPriMedicineModel>> GetDrPriMedicineModelList()
        {
            return _connection.Table<DrPriMedicineModel>().ToListAsync();
        }

        public Task<int> AddDrPriMedicineModelt(DrPriMedicineModel DrPriMedicine)
        {
            return _connection.InsertAsync(DrPriMedicine);
        }

        public Task<int> UpdateOpTicket(OpMethod opMethod)
        {
            return _connection.UpdateAsync(opMethod);          
        }

        //DoctorsDetailModel
        public Task<List<DoctorsDetailModel>> GetDoctorstList()
        {
            return _connection.Table<DoctorsDetailModel>().ToListAsync();
        }

        public Task<int> AddDoctor(DoctorsDetailModel newDoctor)
        {
            return _connection.InsertAsync(newDoctor);
        }
        
        public Task<int> UpdateDoctor(DoctorsDetailModel updateDoctor)
        {
            return _connection.UpdateAsync(updateDoctor);
        }
        
        public Task<int> DeleteDoctor(DoctorsDetailModel doctor)
        {
            return _connection.DeleteAsync(doctor);
        }

        //MedicineModel
        public Task<List<MedicineModel>> GetMedicineList()
        {
            return _connection.Table<MedicineModel>().ToListAsync();
        }

        public Task<int> AddMedicine(MedicineModel newMedicine)
        {
            return _connection.InsertAsync(newMedicine);
        }

        public Task<int> UpdateMedicine(MedicineModel Medicine)
        {
            return _connection.UpdateAsync(Medicine);
        }
        
        public Task<int> DeleteMedicine(MedicineModel Medicine)
        {
            return _connection.DeleteAsync(Medicine);
        }

        //DoctorsPriscriptionModel
        public async Task <DoctorsPriscriptionModel> GetDoctorsPriscription(int patientId)
        {
            List<PriscriptionModelDB> ListP = new List<PriscriptionModelDB>();
            List<PriscriptionModel> MainList = new List<PriscriptionModel>();
            List<DoctorsPriscriptionModelDB> PriscriptionList = new List<DoctorsPriscriptionModelDB>();
            DoctorsPriscriptionModelDB PriscriptionObj = new DoctorsPriscriptionModelDB();
            List<DrPriMedicineModel> MedList = new List<DrPriMedicineModel>();
            var medList = await App.DataBase.GetDrPriMedicineModelList();
            MedList = medList.Where(x => x.patiantId == patientId).ToList();
            ListP = await App.DataBase.GetPriscriptionMedicineList(patientId);

            foreach( var item in ListP)
            {
                MedicineModel sampleMed = new MedicineModel();
                foreach(var med in MedList)
                {
                    if(med.medID == item.MedId && med.patiantId == patientId)
                    {
                        sampleMed = new MedicineModel
                        {
                            ID =med.medID,
                            EXpDate = med.EXpDate,
                            Name = med.Name,
                            Qty = med.Qty,
                            Description = med.Description,
                            patiantId = med.patiantId,
                            Price = med.Price,
                        };
                    }
                }
                //var test = MedList.Where(x => x.patiantId == patientId && x.ID == item.MedId).ToList();
                //sampleMed = MedList.Where(x => x.patiantId == patientId && x.ID ==item.MedId).ToList()[0];
                MainList.Add(new PriscriptionModel{
                    SelectedMedicine = sampleMed,
                    NoDays = item.NoDays,
                    Noon =item.Noon,
                    Morning= item.Morning,
                    Evening= item.Evening,
                    Befor = item.Befor,
                    MedId = item.MedId,
                    Qty = item.Qty,
                    RecordTotal = item.RecordTotal,
                    UnitPrice = item.UnitPrice,
                    patiantId =item.patiantId
                });
            }
            //PriscriptionList.MedList
            PriscriptionList = await App.DataBase.GetDoctorsPriscriptionList();
            PriscriptionObj = PriscriptionList.Where(x => x.PatientId == patientId).ToList()[0];

            return new DoctorsPriscriptionModel
            {
                Total = PriscriptionObj.Total,
                MedList = MainList,
            };
        }

        public async Task<List<DoctorsPriscriptionModelDB>> GetDoctorsPriscriptionList()
        {
            return await _connection.Table<DoctorsPriscriptionModelDB>().ToListAsync();
        }

        public async Task<int> AddDoctorsPriscription(DoctorsPriscriptionModel newItem)
        {
            await App.DataBase.UpdateOpTicket(newItem.PatientDetails);
            int forinkey = newItem.PatientDetails.ID;
            if (!newItem.PatientDetails.IsPharmacyCompleted)
            {               
                foreach (var item in newItem.MedList)
                {
                    PriscriptionModelDB Priscription = new PriscriptionModelDB
                    {
                        Morning = item.Morning,
                        Evening = item.Evening,
                        NoDays = item.NoDays,
                        Noon = item.Noon,
                        UnitPrice = item.UnitPrice,
                        Befor = item.Befor,
                        RecordTotal = item.RecordTotal,
                        patiantId = forinkey,
                        Qty = item.Qty,
                        MedId = item.SelectedMedicine.ID
                    };
                    item.SelectedMedicine.patiantId = forinkey;
                    item.SelectedMedicine.Qty = item.SelectedMedicine.Qty - item.Qty;
                    await App.DataBase.UpdateMedicine(item.SelectedMedicine);
                    await Task.Delay(100);
                    await App.DataBase.AddPriscriptionMedicine(Priscription);
                    await App.DataBase.AddDrPriMedicineModelt(new DrPriMedicineModel
                    {
                        EXpDate = item.SelectedMedicine.EXpDate,
                        Name = item.SelectedMedicine.Name,
                        Qty = item.SelectedMedicine.Qty,
                        Description = item.SelectedMedicine.Description,
                        patiantId = forinkey,
                        Price = item.SelectedMedicine.Price,
                        medID = item.SelectedMedicine.ID,
                    });
                }
                DoctorsPriscriptionModelDB PriscriptionDB = new DoctorsPriscriptionModelDB
                {
                    Total = newItem.Total,
                    PatientId = forinkey,
                };
                return await _connection.InsertAsync(PriscriptionDB);
            }
            else
            {
                DoctorsPriscriptionModelDB PriscriptionDB = new DoctorsPriscriptionModelDB
                {
                    Total = newItem.Total,
                    PatientId = forinkey,
                };
                return await _connection.UpdateAsync(PriscriptionDB);
            }                     
        }

        //PriscriptionModelDB
        public Task<List<PriscriptionModelDB>> GetPriscriptionMedicineList(int PatiantId)
        {
            return _connection.Table<PriscriptionModelDB>().Where(x=>x.patiantId == PatiantId).ToListAsync();
        }

        public Task<int> AddPriscriptionMedicine(PriscriptionModelDB newMedicine)
        {
            return _connection.InsertAsync(newMedicine);
        }
    }
}
