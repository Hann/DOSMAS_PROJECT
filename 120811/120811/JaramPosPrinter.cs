using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.PointOfService;
using System.Data;
using System.IO;

namespace _120811
{
    public class JaramPosPrinter
    {
        PosPrinter m_Printer = null;
        string headerTemplete = "";


        //Constructor
        public JaramPosPrinter()
        {
            prepare();
            createTemplete(@"C:\textTest\text.txt");
        }

        public void printReceipt()
        {
            try
            {
                m_Printer.AsyncMode = true;

                m_Printer.PrintNormal(PrinterStation.Receipt, headerTemplete);
                //m_Printer.PrintNormal(PrinterStation.Receipt, "DOSMAS PROJECT\n");
                //m_Printer.PrintNormal(PrinterStation.Receipt, "한진수\n");
                //m_Printer.PrintNormal(PrinterStation.Receipt, "권기원\n");
                //m_Printer.PrintNormal(PrinterStation.Receipt, "김은숙\n");
                //m_Printer.PrintNormal(PrinterStation.Receipt, "김창주\n");

                //open cash drawer
                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001bp07y");

                //cut paper
                m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|fP");

                m_Printer.AsyncMode = false;
            }
            catch (PosControlException e)
            {
                Console.WriteLine("error");
                Console.WriteLine(e);
            }
        }


        //prepare printer
        private void prepare()
        {
            string strLogicalName = "PosPrinter";
            try
            {
                PosExplorer posExplorer = new PosExplorer();

                DeviceInfo deviceInfo = null;

                try
                {
                    deviceInfo = posExplorer.GetDevice(DeviceType.PosPrinter, strLogicalName);
                    m_Printer = (PosPrinter)posExplorer.CreateInstance(deviceInfo);
                }
                catch (Exception e)
                {
                    Console.WriteLine("error2");
                    Console.WriteLine(e);
                }

                m_Printer.Open();

                m_Printer.Claim(1000);

                m_Printer.DeviceEnabled = true;

            }

            catch (PosControlException e)
            {
                Console.WriteLine("error3");
                Console.WriteLine(e);
            }

            finally
            {
                //release();
            }
        }

        //release printer
        private void release()
        {
            if (m_Printer != null)
            {
                try
                {
                    //Cancel the device
                    m_Printer.DeviceEnabled = false;

                    //Release the device exclusive control right
                    m_Printer.Release();

                    Console.WriteLine("release complete");
                }
                catch (PosControlException)
                {
                }
                finally
                {
                    //Finish using the device
                    m_Printer.Close();
                }
            }
        }

        private void createTemplete(string filePath){
            StreamReader sr = new StreamReader(filePath, System.Text.Encoding.Default);

            String line;
            while ((line = sr.ReadLine()) != null)
            {
                headerTemplete = headerTemplete + "\n" + line;
            }

            sr.Close();
        }

        private void testPrint()
        {

        }

        //text write test
        public void writeTest()
        {
            try
            {
                StreamWriter sw = new StreamWriter(@"C:\textTest\text1.txt");

                sw.WriteLine("테스트입니다");

                sw.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                
            }
        }
    }
}
