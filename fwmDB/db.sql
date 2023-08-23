drop table if exists public."downloadhistory";
drop table if exists public."componentschangeshistory";
drop table if exists public."releasechangeshistory";
drop table if exists public."releasesfiles";
drop table if exists public."componentsreleases";
drop table if exists public."components";
drop table if exists public."uhistory";

drop function if exists public."find_new_releases";
drop function if exists public."find_removed_releases";
drop function if exists public."find_new_components";
drop function if exists public."find_removed_components";

commit;

create table public."uhistory"( 
    id serial primary key,
    d2 timestamp NULL,
    status varchar NULL,
    ipused text NULL,
    ipavailable text[] NULL,
    sizeooffile integer NULL,
    source text,
    info text    
) with (OIDS=FALSE);

commit;

create table public."components" (
    id serial primary key,
    uhistoryid INTEGER REFERENCES "uhistory" (id),
    componentid text,
    componenttype text,
    componenteol text,
    componentname text,
    summary text,
    url text,
    projectlicense text,
    developername text,
    categories text[]
)    with (OIDS=FALSE);



create table public."componentsreleases" (
    id serial primary key,
    uhistoryid INTEGER REFERENCES "uhistory" (id),
    componentid INTEGER REFERENCES "components" (id),

    componentidname text, 
    releaseid text,
    releaseversion text,
    releasetimestamp timestamp ,
    urgency text,
    releaselocation text,
    releasedescription text,
    sizeinstalled integer,
    sizedownload integer,

    downloaded boolean,
    localfilename text
    
)    with (OIDS=FALSE);


create table public."releasesfiles" (
    id serial primary key,
    uhistoryid INTEGER REFERENCES "uhistory" (id),
    componentid INTEGER REFERENCES "components" (id),
    releaseid INTEGER REFERENCES "componentsreleases" (id),

    checksumtype text,
    checksumfilename text,
    checksumtarget text,
    checksumvalue text
    
)    with (OIDS=FALSE);

-- История изменений по релизам
create table public."releasechangeshistory" (
    id serial primary key,
    uhistoryidcur INTEGER REFERENCES "uhistory" (id),
    uhistoryidprev INTEGER REFERENCES "uhistory" (id),
    
    componentname text,
    releasetimestamp timestamp,
    releaseversion text,
    releasedescription text,
    operationtype text
      
)    with (OIDS=FALSE);


-- История изменений по компонентам
create table public."componentschangeshistory" (
    id serial primary key,
    uhistoryidcur INTEGER REFERENCES "uhistory" (id),
    uhistoryidprev INTEGER REFERENCES "uhistory" (id),    
    componentid text,
    operationtype text        
)    with (OIDS=FALSE);

-- 

create table public."downloadhistory" (
    id serial primary key,
    uhistoryid INTEGER REFERENCES "uhistory" (id),
    componentid INTEGER REFERENCES "components" (id),

    releaseid text,
    releaseversion text,
    releasetimestamp timestamp ,
    urgency text,
    releaselocation text,
    releasedescription text,
    sizeinstalled integer,
    sizedownload integer,

    downloaded timestamp,
    localfilename text
    
)    with (OIDS=FALSE);




Create OR REPLACE FUNCTION find_new_releases(uid1 int, uid2 int) RETURNS table (releasetimestamp timestamp, releaseversion text, releasedescription text, componentname text, componentid text )
language plpgsql
as $$
begin
return query 
SELECT t1.releasetimestamp , t1.releaseversion, t1.releasedescription , t2.componentname, t2.componentid  FROM componentsreleases t1, components t2  where t2.uhistoryid = uid1 and t1.componentid = t2.id 
except 
SELECT t1.releasetimestamp , t1.releaseversion , t1.releasedescription , t2.componentname , t2.componentid FROM componentsreleases t1, components t2  where t2.uhistoryid = uid2 and t1.componentid = t2.id;
end; $$;



CREATE OR REPLACE FUNCTION find_removed_releases(uid1 int, uid2 int) RETURNS table (releasetimestamp timestamp, releaseversion text, releasedescription text, componentname text, componentid text )
language plpgsql
as $$
begin
return query 
SELECT t1.releasetimestamp , t1.releaseversion , t1.releasedescription , t2.componentname, t2.componentid  FROM componentsreleases t1, components t2  where t2.uhistoryid = uid2 and t1.componentid = t2.id 
except
SELECT t1.releasetimestamp , t1.releaseversion , t1.releasedescription , t2.componentname, t2.componentid  FROM componentsreleases t1, components t2  where t2.uhistoryid = uid1 and t1.componentid = t2.id;
end; $$;

CREATE OR REPLACE FUNCTION find_new_components(uid1 int, uid2 int) RETURNS table (componentid text)
language plpgsql
as $$
begin
return query 
SELECT t1.componentid  FROM components t1 where t1.uhistoryid = uid1  
except 
SELECT t1.componentid  FROM components t1 where t1.uhistoryid = uid2
order by componentid; 
end; $$;


CREATE OR REPLACE FUNCTION find_removed_components(uid1 int, uid2 int) RETURNS table (componentid text)
language plpgsql
as $$
begin
return query 
SELECT t1.componentid  FROM components t1 where t1.uhistoryid = uid2  
except 
SELECT t1.componentid  FROM components t1 where t1.uhistoryid = uid1
order by componentid; 
end; $$;

-- end

