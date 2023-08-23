using System;
using System.Data.Common;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

using Microsoft.EntityFrameworkCore;
using fwmonitor.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Deployment.Compression.Cab;

namespace fwmonitor
{
    public static class Parser
    {
        //public static DB.uiContext db;
        //текущая история в которой работаем
        //public static DB.Uhistory u;

        public static void ParseComponent(fwmonitor.DB.uiContext db, Uhistory u, XmlElement xnode)
        {
            //var db = Program.DB;

            DB.Component c = new Component();
            c.Uhistory=u;
            XmlNode? attr = xnode.Attributes.GetNamedItem("date_eol");

            if (attr!=null)
            {
                //c.En
                c.Componenteol=attr.Value;
            }

            attr = xnode.Attributes.GetNamedItem("type");
            if (attr!=null)
            {
                // всегда firmware игнорим
                //c.En
                //c.Componenteol=attr.Value;
            }

            foreach (XmlNode cn in xnode.ChildNodes)
            {

                if (cn.Name == "id")
                {
                    c.Componentid=cn.InnerText;
                }

                if (cn.Name == "name")
                {

                    c.Componentname=cn.InnerText;
                }

                if (cn.Name == "project_license")
                {

                    c.Projectlicense=cn.InnerText;
                }

                if (cn.Name == "developer_name")
                {

                    c.Developername=cn.InnerText;
                }

                if (cn.Name == "url")
                {

                    c.Url=cn.InnerText;
                }

                if (cn.Name == "summary")
                {

                    c.Summary=cn.InnerText;
                }

                if (cn.Name=="categories")
                {
                    c.Categories=new string[] { };
                    foreach (var cn2 in cn.ChildNodes.OfType<XmlNode>())
                    {
                        if (cn2.Name == "category")
                        {
                            c.Categories.Append(cn2.InnerText);
                        }
                    }
                }

                if (cn.Name=="releases")
                {
                    foreach (var cn2 in cn.ChildNodes)
                    {
                        DB.Componentsrelease cr = new Componentsrelease();
                        cr.Component=c;
                        cr.Uhistory=u;

                        XmlNode? xn = cn2 as XmlNode;

                        if (xn!=null)
                        {

                            XmlNode? attr1 = xn.Attributes.GetNamedItem("id");

                            if (attr1!=null)
                            {
                                //c.En
                                cr.Releaseid=attr1.InnerText;
                            }

                            attr1 = xn.Attributes.GetNamedItem("version");
                            if (attr1!=null)
                            {
                                //c.En
                                cr.Releaseversion=attr1.InnerText;
                            }

                            attr1 = xn.Attributes.GetNamedItem("timestamp");
                            if (attr1!=null)
                            {
                                //c.En
                                cr.Releasetimestamp=Utils.UnixTimestampToDateTime(Double.Parse(attr1.InnerText));
                            }

                            attr1 = xn.Attributes.GetNamedItem("urgency");
                            if (attr1!=null)
                            {
                                //c.En
                                cr.Urgency=attr1.InnerText;
                            }

                            //   ParseRelease(xn);                 
                            foreach (var cn3 in xn.ChildNodes.OfType<XmlNode>())
                            {
                                //Console.WriteLine(cn3.Name);

                                if (cn3.Name=="location")
                                {
                                    cr.Releaselocation=cn3.InnerText;
                                }

                                if (cn3.Name=="checksum")
                                {
                                    DB.Releasesfile f = new Releasesfile();
                                    f.Release=cr;
                                    f.Uhistory=u;
                                    f.Component=c;
                                    f.Checksumfilename=""+cn3.Attributes.GetNamedItem("filename")?.Value;
                                    f.Checksumtarget=""+cn3.Attributes.GetNamedItem("target")?.Value;
                                    f.Checksumtype=""+cn3.Attributes.GetNamedItem("type")?.Value;
                                    f.Checksumvalue=cn3.Value;
                                    db.Releasesfiles.Add(f);
                                }

                                if (cn3.Name=="description")
                                {
                                    cr.Releasedescription=cn3.InnerText;
                                }

                                if (cn3.Name=="size")
                                {
                                    if (cn3.Attributes.GetNamedItem("type")?.Value=="installed")
                                    {
                                        cr.Sizeinstalled=Int32.Parse(cn3.InnerText);
                                    }

                                    if (cn3.Attributes.GetNamedItem("type")?.Value=="download")
                                    {
                                        //cr.
                                        cr.Sizedownload=Int32.Parse(cn3.InnerText);
                                    }

                                }

                                if (cn3.Name=="issues")
                                {

                                }

                                if (cn3.Name=="url")
                                {

                                }

                            }

                            c.Componentsreleases.Add(cr);
                            //db.SaveChanges();
                            //return;
                        }
                    }
                }

            }
            db.Components.Add(c);
        }

        public static string ImportFile(string path, string comment, Uhistory u)
        {
            var db = Program.DB;
            var db2 = Program.DB;

            Log.L("Импортируем файл "+path+", комментарий "+comment);
            string result = "";
            result=DateTime.Now.ToString("yyyyMMdd HH:mm")+"\tstarted\r\n";

            XmlDocument xDoc = new XmlDocument();

            xDoc.Load(path);

            //var u = new Uhistory();
            u.D2=DateTime.Now;            
            u.Sizeooffile= (int)(new FileInfo(path).Length % Int32.MaxValue); ;//File.Size(path);
            u.Info=comment;
            db.Uhistories.Add(u);
            db.SaveChanges();

            result+="Update #"+u.Id+"\r\n";

            XmlElement? xRoot = xDoc.DocumentElement;
            if (xRoot==null)
            {
                result+="incorrect format\r\n";
                u.Status="failed";
                db.SaveChanges();
                result+="error";
            }
            else
            {

                int packet = 0;
                db.ChangeTracker.AutoDetectChangesEnabled = false;
                foreach (XmlElement xnode in xRoot)
                {

                    if (xnode.Name=="component")
                    {
                        ParseComponent(db, u, xnode);
                        if (packet%100==0)
                            db.SaveChanges();
                        packet++;

                    }
                }
                db.ChangeTracker.AutoDetectChangesEnabled = true;
            }// 

            result+=DateTime.Now+"\tfinished\r\n";
            u.Info=u.Info+"\r\n"+result;
            db.SaveChanges();
            Log.L("Импорт завершен");

            #region analyze using db
            // Проанализировать сразу и записать в соответствующие таблицы дельты
            var uprevr = db.Uhistories.Where(x => x.Id<u.Id).OrderByDescending(x => x.Id);
            if (uprevr!=null && uprevr.Count()>0)
            {
                var uprev = uprevr.First();
                Log.L("Записываем разницу текущий ID:"+u.Id+", предыдущий: "+uprev.Id);
                if (uprev!=null)
                {
                    db.Database.ExecuteSqlRaw("delete from releasechangeshistory where uhistoryidcur='"+u.Id+"' and uhistoryidprev='"+uprev.Id+"' and operationtype='new'");
                    string s = "insert into releasechangeshistory (uhistoryidcur ,     uhistoryidprev ,     componentname ,     releasetimestamp ,     releaseversion  ,     releasedescription, operationtype ) ( select  "+u.Id.ToString()+" as uhistoryidcur, "+uprev.Id.ToString()+" as uhistoryidprev, componentname ,     releasetimestamp ,     releaseversion  ,     releasedescription, 'new' as opertiontype from find_new_releases("+u.Id.ToString()+", "+uprev.Id.ToString()+") order by releasetimestamp desc  )";
                    db.Database.ExecuteSqlRaw(s);

                    db.Database.ExecuteSqlRaw("delete from releasechangeshistory where uhistoryidcur='"+u.Id+"' and uhistoryidprev='"+uprev.Id+"' and operationtype='removed'");
                    s = "insert into releasechangeshistory (uhistoryidcur ,     uhistoryidprev ,     componentname ,     releasetimestamp ,     releaseversion  ,     releasedescription, operationtype ) ( select  "+u.Id.ToString()+" as uhistoryidcur, "+uprev.Id.ToString()+" as uhistoryidprev, componentname ,     releasetimestamp ,     releaseversion  ,     releasedescription, 'removed' as opertiontype from find_removed_releases("+u.Id.ToString()+", "+uprev.Id.ToString()+") order by releasetimestamp desc  )";
                    db.Database.ExecuteSqlRaw(s);

                    //components
                    db.Database.ExecuteSqlRaw("delete from componentschangeshistory where uhistoryidcur='"+u.Id+"' and uhistoryidprev='"+uprev.Id+"' and operationtype='new'");
                    s = "insert into componentschangeshistory (uhistoryidcur ,     uhistoryidprev ,     componentid, operationtype ) ( select  "+u.Id.ToString()+" as uhistoryidcur, "+uprev.Id.ToString()+" as uhistoryidprev, componentid , 'new' as opertiontype from find_new_components("+u.Id.ToString()+", "+uprev.Id.ToString()+") )";
                    db.Database.ExecuteSqlRaw(s);

                    db.Database.ExecuteSqlRaw("delete from componentschangeshistory where uhistoryidcur='"+u.Id+"' and uhistoryidprev='"+uprev.Id+"' and operationtype='removed'");
                    s = "insert into componentschangeshistory (uhistoryidcur ,     uhistoryidprev ,     componentid, operationtype ) ( select  "+u.Id.ToString()+" as uhistoryidcur, "+uprev.Id.ToString()+" as uhistoryidprev, componentid , 'removed' as opertiontype from find_removed_components("+u.Id.ToString()+", "+uprev.Id.ToString()+") )";
                    db.Database.ExecuteSqlRaw(s);
                }
                #endregion

                if(Program.S.DownloadCab){
                    // НОВЫХ загрузка кабов 
                  int i = 1;
                  foreach (var d in db.Releasechangeshistories.Where(x => x.Uhistoryidcur==u.Id && x.Uhistoryidprev==uprev.Id && x.Operationtype=="new")) {
                    var tod=db2.Componentsreleases.Where(x => x.Uhistoryid==u.Id && x.Releaseversion==d.Releaseversion && x.Releasedescription==d.Releasedescription && x.Releasetimestamp==d.Releasetimestamp);
                       int i2=1;
                    foreach (var tod2 in tod) {
                        Console.WriteLine(i+"/"+i2 +" to download "+tod2.Releaselocation);
                            DownloadRelease(tod2.Releaselocation);
                            i++;
                            i2++;
                     }
                }                
                }
            }

            

            return result;
        }


        public static void DownloadRelease(string url) {
            if (Program.S.StorageLocation=="") {
                Log.L("Не задано хранилище");
                return;
            }
            try
            {
                if (!Directory.Exists(Program.S.StorageLocation))
                {
                    Directory.CreateDirectory(Program.S.StorageLocation);
                    
                    if(!Directory.Exists(Program.S.StorageLocation+"/diffsize"))
                        Directory.CreateDirectory(Program.S.StorageLocation+"/diffsize");
                }
            }
            catch (Exception e) {
                Log.L("Ошибка создания директории хранилища "+e.Message);
                return;
            }

            string fn = Utils.GetFileNameFromUrl(url);
            string ffn = Program.S.StorageLocation+"/"+fn;
            //
            if (File.Exists(ffn))
            {
                // check size
                Log.L("Файл существует пропускаем");
            }
            else {
                //убого
                try
                {
                    Log.L("Загружаем файл "+ffn+" из "+url);
                    var client = new WebClient();
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                    client.DownloadFile(url, ffn);
                } catch (Exception e)
                {
                    Log.L("Ошибка загрузки :"+e.Message);
                }

                if (File.Exists(ffn)) {
                    // Все загружено отправляем на парсинг
                    //ParseRelease(ffn);
                }

            }

        }

        public static void ParseRelease(string fn) {
            // поискать в releases
            var ci = new CabInfo(fn);
            foreach (var u in ci.GetFiles()) {
                // 
                // check for UEFI inside
                Log.L("\t"+fn+"\t"+u.FullName+"\t" +u.Length);

                if (Program.S.ParseUEFI) { 
                // парсинг UEFI 
                }
                
            }

        }
        


    }
}
