using CTQuanLyTiecCuoi.Entities;
using CTQuanLyTiecCuoi.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CTQuanLyTiecCuoi.Views;

namespace CTQuanLyTiecCuoi
{
    static class Program
    {
        public static string connectionString;
        public static UserRepository userRepository;
        public static HallRepository hallRepository;
        public static ServiceRepository serviceRepository;
        public static DishRepository dishRepository;
        public static ReservationRepositoy reservationRepositoy;
        public static CustomerRepository customerRepository;
        public static BillingRepository billingRepository;
        public static Session session;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string path = Directory.GetCurrentDirectory();
            string dataPath = path.Replace("\\bin\\Debug", "\\App_Data");
            AppDomain.CurrentDomain.SetData("DataDirectory", dataPath);

            connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            session = new Session();
            userRepository = new UserRepository(connectionString);
            hallRepository = new HallRepository(connectionString);
            serviceRepository = new ServiceRepository(connectionString);
            dishRepository = new DishRepository(connectionString);
            reservationRepositoy = new ReservationRepositoy(connectionString);
            customerRepository = new CustomerRepository(connectionString);
            billingRepository = new BillingRepository(connectionString);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmDangNhap());
            //Application.Run(new frmHoaDon());
            Application.Exit();
        }
    }
}
