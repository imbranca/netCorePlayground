
SELECT TOP 1000
    c.Id,
    c.Title,
    c.Price,
    cat.Name,
    i.Initials,
    u.UserName AS InstructorName
FROM Courses c
INNER JOIN Instructors i ON c.InstructorId = i.InstructorId
INNER JOIN AspNetUsers u ON i.UserId = u.Id
INNER JOIN Categories cat ON c.CategoryId = cat.CategoryId;


SELECT * FROM Students s
SELECT * FROM Courses c

select * from Students s
inner join AspNetUsers u on s.UserId = u.Id
where u.RoleId = 3

select * from Enrollments order by EnrollmentDate desc

select * from Students s
left join AspNetUsers u on s.UserId = u.Id
where u.RoleId = 3

SELECT * FROM Courses c


SELECT u.Id AS instructor_id,
       u.UserName AS instructor_name,
       COUNT(e.StudentId) AS total_students
FROM AspNetUsers u
JOIN Instructors st on st.UserId = u.Id
JOIN courses c ON c.InstructorId = st.InstructorId
JOIN enrollments e ON e.CourseId = c.Id
WHERE u.RoleId = 2
ORDER BY total_students DESC;

--get amount of user enrolled
select c.Title, u.UserName, COUNT(e.StudentId) from AspNetUsers u
join Instructors i on u.Id = i.UserId
join courses c on c.InstructorId = i.InstructorId
join Enrollments e on e.CourseId = c.Id
group by c.Title, u.UserName