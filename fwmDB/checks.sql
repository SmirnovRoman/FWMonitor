select * from find_new_components(2, 1)
select * from find_removed_components(2, 1)

select * from find_new_components(3, 2)
select * from find_removed_components(3, 2)

select * from find_removed_releases(2, 1)
except

select * from find_new_releases(2, 1)
select * from find_removed_releases(2, 1)


select * from find_new_releases(3, 2)
select * from find_new_releases(3, 1)

select * from componentsreleases c1 where releaseversion ='70656' and componentid in (select id from components c  where componentid ='com.dell.uefib6972c97.firmware')

select * from componentsreleases c1 where releaseversion ='71680' and componentid in (select id from components c  where componentid ='com.dell.uefi8c955858.firmware')

-- проверка что 66305 удќлили
select * from componentsreleases c1 where componentid in (select id from components c  where componentid ='com.dell.uefi4c9bb0f4.firmware') order by uhistoryid , releaseversion



-- јналитика
select developername, count(*) as c from components group by developername  order by c desc

