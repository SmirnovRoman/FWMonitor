using System.Data.Common;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;

namespace fwmonitor {
    public class Startup{

        public string FWInfo(string id){


            return "";
        }


	   public void Configure(IApplicationBuilder app){


            app.Run(async c => {
                
                
                var response = c.Response;

                response.Headers["Content-Type"]="text/html; charset=utf-8";
                //response.Headers.Append("secret-id", "256");

                string body="FWMonitor<hr size=1>";
                //var db;
                body+=c.Request.QueryString;

                

                var db=Program.DB;
                var db2=Program.DB;
                if(db==null){
                    body+="cant connect to db";
                    //return await response.WriteAsync(body);                    
                } else {

                if(c.Request.QueryString.Value.Length>4 && c.Request.QueryString.Value.Substring(0,"?id=".Length)=="?id="){
                        // инф. о прошивке
                        string id=c.Request.QueryString.Value.Substring("?=id".Length);
                         
                        var r=db2.Components.Where(x=>x.Componentid==id);//.Include(x=>x.Componentsreleases);//.Include("Releasesfiles");
                        
                        JsonSerializerOptions jso = new JsonSerializerOptions();
                        jso.WriteIndented=true;
                        jso.MaxDepth=3;
                        jso.UnknownTypeHandling=System.Text.Json.Serialization.JsonUnknownTypeHandling.JsonElement;
                        jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

                        body+= "<pre>"+JsonSerializer.Serialize(r, jso)+"</pre>";

                        goto label_a;
                } 


                body+=@"<table border=1 bordercolor=green style='font-family:verdana' cellspacing=1 cellpadding=1 width='100%'>
                        <tr>
                            <td>#</td>
                            <td>Время</td>
                            <td>Инф.</td>
                            <td>Кол-во компонент</td>
                            <td>Кол-во релизов</td>
                            <td>Размер</td>
                            <td>Новые компоненты</td>
                            <td>Удаленные компоненты</td>
                            <td>Новые Релизы</td>
                            <td>Удаленные Релизы</td>
                        </tr>
                ";
                int i=1;
                int previd=0;
                foreach(var uh in db.Uhistories.OrderBy(u=>u.D2)){
                    string comp="";
                    string comp2="";
                    string comp3="";
                    string comp4 = "";
                     if (previd!=0){
                        Programm.CompareResult cr=null;
                            // временно обратная сортировка
                            /* cr=Programm.Compare(uh.Id,previd);
                            comp=String.Join("<br>", cr.NewIds);
                            foreach(var t in cr.NewIds){
                                comp+="<a href='?id="+HttpUtility.UrlEncode(t)+"'>"+t+"</a> ";
                            }

                            foreach(var t in cr.RemovedIds){
                                comp2+="<a href='?id="+HttpUtility.UrlEncode(t)+"'>"+t+"</a> ";
                            }
                            */
                            /* TODO:
                            var r=db.Database.QueryString("select * from find_new_releases("+uh.Id+", "+previd+") ");
                            foreach(var t in r){
                                comp3+="<a href='?release="+HttpUtility.UrlEncode(t)+"'>"+t+"</a> ";
                            }
                            */
                            var r0 = db2.Componentschangeshistories.Where(x => x.Uhistoryidcur==uh.Id && x.Uhistoryidprev==previd && x.Operationtype=="new");
                            foreach (var r2 in r0)
                            {
                                    comp+="<a href='?id="+HttpUtility.UrlEncode(r2.Componentid)+"'>"+r2.Componentid+"</a> ";
                            }

                            var r01 = db2.Componentschangeshistories.Where(x => x.Uhistoryidcur==uh.Id && x.Uhistoryidprev==previd && x.Operationtype=="removed");
                            foreach (var r2 in r01)
                            {
                                  comp2+="<a href='?id="+HttpUtility.UrlEncode(r2.Componentid)+"'>"+r2.Componentid+"</a> ";
                            }

                            var r= db2.Releasechangeshistories.Where(x=>x.Uhistoryidcur==uh.Id && x.Uhistoryidprev==previd && x.Operationtype=="new");
                            foreach(var r2 in r){ 
                                comp3+=r2.Releasetimestamp.Value.ToString("yyyyMMdd HH:mm")+"\t"+r2.Componentname+"\t"+r2.Releaseversion+"\t"+r2.Releasedescription+"<br>";
                            }

                            var r02 = db2.Releasechangeshistories.Where(x => x.Uhistoryidcur==uh.Id && x.Uhistoryidprev==previd && x.Operationtype=="removed");
                            foreach (var r2 in r02)
                            {
                                comp4+=r2.Releasetimestamp.Value.ToString("yyyyMMdd HH:mm")+"\t"+r2.Componentname+"\t"+r2.Releaseversion+"\t"+r2.Releasedescription+"<br>";
                            }
                        }
                    
                    body+="<tr><td>"+i+"</td>"+
                    "<td valign=top>"+uh.D2.Value.ToString("yyyyMMdd HH:mm")+"</td>"+
                    "<td valign=top>"+uh.Info+"</td>"+
                    "<td valign=top>"+db2.Components.Where(x=>x.Uhistoryid==uh.Id).Count()+"</td>"+
                    "<td valign=top>"+db2.Componentsreleases.Where(x=>x.Uhistoryid==uh.Id).Count()+"</td>"+
                    "<td valign=top>"+uh.Sizeooffile+"</td>"+
                    "<td valign=top>"+comp+"</td>"+
                    "<td valign=top>"+comp2+"</td>"+
                    "<td valign=top><small>"+comp3+"</small></td>"+
                    "<td valign=top><small>"+comp4+"</small></td>"+
                    "</tr>";
                    i++;
                    previd=uh.Id;
                }                
                body+="</table>";
                label_a:;
                }        
                await response.WriteAsync(body,Encoding.UTF8);
            });
            
        }
    }
}