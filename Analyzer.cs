namespace fwmonitor{

public static partial class Programm{

    public class CompareResult{
        public string[] NewIds = new string[]{};
        public string[] RemovedIds = new string[]{};
        //releases
        public string[] NewReleases = new string[]{};
        public string[] RemovedReleases = new string[]{};
        public string[] ChangedReleases = new string[]{};
    }
public static CompareResult Compare(int uid1, int uid2){
        // поискать разные размеры для одних и тех же релизов

        string r="";
        var db3 = Program.DB;
        var db4 = Program.DB;
        CompareResult cr= new CompareResult();
        
        List<string> NewIds = new List<string>();
        List<string> RemovedIds = new List<string>();

        List<string> NewReleases = new List<string>();
        List<string> RemovedReleases = new List<string>();
        List<string> ChangedReleases = new List<string>();

        //компоненты
        foreach(var c1 in db3.Components.Where(x=>x.Uhistoryid==uid1)){
            
            var v=db4.Components.Where(x=>x.Componentid==c1.Componentid && x.Uhistoryid==uid2   );
            if(v.Count()>0){
                
            } else {
                //не найдена
                    NewIds.Add(c1.Componentid);
                    continue;
            }                                 
        }

        //Обратная
        foreach(var c1 in db3.Components.Where(x=>x.Uhistoryid==uid2)){
            
            var v=db4.Components.Where(x=>x.Componentid==c1.Componentid && x.Uhistoryid==uid1   );
            if(v.Count()>0){
                
            } else {
                //не найдена
                    RemovedIds.Add(c1.Componentid);
                    continue;
            }                                 
        }
        cr.NewIds=NewIds.ToArray();
        cr.RemovedIds=RemovedIds.ToArray();
        
/* тупиковый путь

        
        //релизы
        foreach(var c1 in db3.Componentsreleases.Where(x=>x.Uhistoryid==uid1)){
            //c1.Component.Load();            
            var v=db4.Componentsreleases.Where(x=>x.Component.Componentid==c1.Component.Componentid && x.Uhistoryid==uid2 && x.Releaseversion==c1.Releaseversion);
            if(v.Count()>0){
                
            } else {
                //не найдена
                    NewReleases.Add(c1.Releasedescription);
                    continue;
            }                                 
        }

        cr.NewReleases=NewReleases.ToArray();
*/
        
        
        db3.Dispose();
        db3=null;
        db4.Dispose();
        db4=null;
        return cr;
    }

}

}