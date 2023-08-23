#0 3 * * * pg_dump --dbname=$MYDB | gzip > ~/backup/db/$(date +%Y-%m-%d).psql.gz
pg_dump --host=localhost -U dbfwmon -d dbfwmon -f DB/dump.sql 