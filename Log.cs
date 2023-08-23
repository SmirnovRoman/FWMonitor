using System;
using System.IO;


namespace fwmonitor {

public static class Log{
    public static bool Show2Console=false;
    static int iLog=1;
    
    public static void L(string s){
        string s2=iLog+"\t"+DateTime.Now.ToString("yyyyMMdd HH:mm")+"\t"+s.Replace("\n"," ")+"\r\n";
        iLog++;
        if(Program.S.Show2Console){
            Console.WriteLine(s2);
        }   
     
        try{
            System.IO.File.AppendAllText(Program.S.LogFile,s2, System.Text.Encoding.UTF8);
        } catch(Exception e){
            Console.WriteLine("Log error:"+e.Message);
        }
    }
}

}