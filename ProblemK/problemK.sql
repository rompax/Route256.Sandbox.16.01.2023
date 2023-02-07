select distinct users.id, users.name from users 
left join orders on users.id = orders.user_id 
where  orders.id  IS NOT NULL 
order by users.name, users.id