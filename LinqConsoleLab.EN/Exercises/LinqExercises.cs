using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using LinqConsoleLab.EN.Data;

namespace LinqConsoleLab.EN.Exercises;

public sealed class LinqExercises
{
    /// <summary>
    /// Task:
    /// Find all students who live in Warsaw.
    /// Return the index number, full name, and city.
    ///
    /// SQL:
    /// SELECT IndexNumber, FirstName, LastName, City
    /// FROM Students
    /// WHERE City = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Task01_StudentsFromWarsaw()
    {
        var method = UniversityData.Students.Where(s => s.City.Equals("Warsaw"))
            .Select(s => $"{s.IndexNumber}, {s.FirstName}, {s.LastName}, {s.City}");

        return method;
    }

    /// <summary>
    /// Task:
    /// Build a list of all student email addresses.
    /// Use projection so that you do not return whole objects.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Students;
    /// </summary>
    public IEnumerable<string> Task02_StudentEmailAddresses()
    {
        var method = UniversityData.Students.Select(s => s.Email);

        return method;
    }

    /// <summary>
    /// Task:
    /// Sort students alphabetically by last name and then by first name.
    /// Return the index number and full name.
    ///
    /// SQL:
    /// SELECT IndexNumber, FirstName, LastName
    /// FROM Students
    /// ORDER BY LastName, FirstName;
    /// </summary>
    public IEnumerable<string> Task03_StudentsSortedAlphabetically()
    {
        var method = UniversityData.Students.OrderBy(s=> s.LastName).ThenBy(s=> s.FirstName)
            .Select(s=> $"{s.IndexNumber}, {s.FirstName}, {s.LastName}");

        return method;
    }

    /// <summary>
    /// Task:
    /// Find the first course from the Analytics category.
    /// If such a course does not exist, return a text message.
    ///
    /// SQL:
    /// SELECT TOP 1 Title, StartDate
    /// FROM Courses
    /// WHERE Category = 'Analytics';
    /// </summary>
    public IEnumerable<string> Task04_FirstAnalyticsCourse()
    {
        var res = UniversityData.Courses.Where(c => c.Category.Equals("Analytics"))
            .Select(c => new {c.Title, c.StartDate})
            .FirstOrDefault();

        if (res != null)
        {
            return [$"{res.Title}, {res.StartDate}"];
        }

        return ["Course does not exist"];

    }

    /// <summary>
    /// Task:
    /// Check whether there is at least one inactive enrollment in the data set.
    /// Return one line with a True/False or Yes/No answer.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Enrollments
    ///     WHERE IsActive = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Task05_IsThereAnyInactiveEnrollment()
    {
        var method = UniversityData.Enrollments.Any(e => !e.IsActive);
        if (method)
        {
            return ["True"];
        }
        
        return ["False"];
    }

    /// <summary>
    /// Task:
    /// Check whether every lecturer has a department assigned.
    /// Use a method that validates the condition for the whole collection.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Department)
    /// THEN 1 ELSE 0 END
    /// FROM Lecturers;
    /// </summary>
    public IEnumerable<string> Task06_DoAllLecturersHaveDepartment()
    {
        var method = (UniversityData.Lecturers.Count(l => !string.IsNullOrEmpty(l.Department)) ==
                      UniversityData.Lecturers.Count)
            ? true
            : false;

        if (method)
        {
            return ["True"];
        }

return ["False"];
    }

    /// <summary>
    /// Task:
    /// Count how many active enrollments exist in the system.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Enrollments
    /// WHERE IsActive = 1;
    /// </summary>
    public IEnumerable<string> Task07_CountActiveEnrollments()
    {
        var method = UniversityData.Enrollments.Count(e => e.IsActive);

        return [$"{method}"];
    }

    /// <summary>
    /// Task:
    /// Return a sorted list of distinct student cities.
    ///
    /// SQL:
    /// SELECT DISTINCT City
    /// FROM Students
    /// ORDER BY City;
    /// </summary>
    public IEnumerable<string> Task08_DistinctStudentCities()
    {
        var method = UniversityData.Students.OrderBy(s=> s.City).Select(s=> s.City).Distinct();
        
        return method;
    }

    /// <summary>
    /// Task:
    /// Return the three newest enrollments.
    /// Show the enrollment date, student identifier, and course identifier.
    ///
    /// SQL:
    /// SELECT TOP 3 EnrollmentDate, StudentId, CourseId
    /// FROM Enrollments
    /// ORDER BY EnrollmentDate DESC;
    /// </summary>
    public IEnumerable<string> Task09_ThreeNewestEnrollments()
    {
        var method = UniversityData.Enrollments.OrderByDescending(e => e.EnrollmentDate)
            .Select(e=> $"{e.EnrollmentDate}, {e.StudentId}, {e.CourseId}").Take(3);

        return method;

    }

    /// <summary>
    /// Task:
    /// Implement simple pagination for the course list.
    /// Assume a page size of 2 and return the second page of data.
    ///
    /// SQL:
    /// SELECT Title, Category
    /// FROM Courses
    /// ORDER BY Title
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Task10_SecondPageOfCourses()
    {
        var method = UniversityData.Courses.OrderBy(c=> c.Title)
            .Select(c=> $"{c.Title}, {c.Category}").Skip(2).Take(2);
        
        return method;
    }

    /// <summary>
    /// Task:
    /// Join students with enrollments by StudentId.
    /// Return the full student name and the enrollment date.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, e.EnrollmentDate
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId;
    /// </summary>
    public IEnumerable<string> Task11_JoinStudentsWithEnrollments()
    {
       var method = UniversityData.Students.Join(UniversityData.Enrollments, s=> s.Id, e=> e.StudentId
       , (s, e)=> new{s.FirstName, s.LastName, e.EnrollmentDate}).Select(e=> $"{e.FirstName} {e.LastName} {e.EnrollmentDate}");

       return method;
    }

    /// <summary>
    /// Task:
    /// Prepare all student-course pairs based on enrollments.
    /// Use an approach that flattens the data into a single result sequence.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, c.Title
    /// FROM Enrollments e
    /// JOIN Students s ON s.Id = e.StudentId
    /// JOIN Courses c ON c.Id = e.CourseId;
    /// </summary>
    public IEnumerable<string> Task12_StudentCoursePairs()
    {
        var method = UniversityData.Enrollments.Join(UniversityData.Students, e=> e.StudentId, s=> s.Id
        , (e, s)=> new{ e, s}).Join(UniversityData.Courses, e=> e.e.CourseId, c=> c.Id, (e, c)=> 
            new {e.s.FirstName, e.s.LastName, c.Title}
            ).Select(e=> $"{e.FirstName} {e.LastName} {e.Title}");

        return method;
    }

    /// <summary>
    /// Task:
    /// Group enrollments by course and return the course title together with the number of enrollments.
    ///
    /// SQL:
    /// SELECT c.Title, COUNT(*)
    /// FROM Enrollments e
    /// JOIN Courses c ON c.Id = e.CourseId
    /// GROUP BY c.Title;
    /// </summary>
    public IEnumerable<string> Task13_GroupEnrollmentsByCourse()
    {
        var method = UniversityData.Enrollments.Join(UniversityData.Courses, e => e.CourseId, c => c.Id, (e, c) =>
            new { e, c }).GroupBy(x => x.c.Title).Select(e => $"{e.Key}, {e.Count()}");

        return method;
    }

    /// <summary>
    /// Task:
    /// Calculate the average final grade for each course.
    /// Ignore records where the final grade is null.
    ///
    /// SQL:
    /// SELECT c.Title, AVG(e.FinalGrade)
    /// FROM Enrollments e
    /// JOIN Courses c ON c.Id = e.CourseId
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY c.Title;
    /// </summary>
    public IEnumerable<string> Task14_AverageGradePerCourse()
    {
        var method = UniversityData.Enrollments.Join(UniversityData.Courses, e=> e.CourseId, c=> c.Id,
            (e,c)=> new{e,c}).Where(e=> e.e.FinalGrade != null).GroupBy(x=> x.c.Title).
            Select(e=> $"{e.Key}, {e.Average(e=> e.e.FinalGrade)}");

        return method;
    }

    /// <summary>
    /// Task:
    /// For each lecturer, count how many courses are assigned to that lecturer.
    /// Return the full lecturer name and the course count.
    ///
    /// SQL:
    /// SELECT l.FirstName, l.LastName, COUNT(c.Id)
    /// FROM Lecturers l
    /// LEFT JOIN Courses c ON c.LecturerId = l.Id
    /// GROUP BY l.FirstName, l.LastName;
    /// </summary>
    public IEnumerable<string> Task15_LecturersAndCourseCounts()
    {
        var method = UniversityData.Lecturers.GroupJoin(UniversityData.Courses, l => l.Id, c => c.LecturerId,
            (l, c) => new { l, c }).Select(e => $"{e.l.FirstName}, {e.l.LastName}, {e.c.Count()}");
    
        return method;
    }

    /// <summary>
    /// Task:
    /// For each student, find the highest final grade.
    /// Skip students who do not have any graded enrollment yet.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, MAX(e.FinalGrade)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY s.FirstName, s.LastName;
    /// </summary>
    public IEnumerable<string> Task16_HighestGradePerStudent()
    {
        var method = UniversityData.Students.Join(UniversityData.Enrollments, s=> s.Id, e=> e.StudentId,
            (s,e)=> new {s, e}).Where(e=> e.e.FinalGrade != null).GroupBy(e=> new {e.s.FirstName, e.s.LastName})
            .Select(e=> $"{e.Key.FirstName} {e.Key.LastName}, {e.Max(e=> e.e.FinalGrade)}");

        return method;
    }

    /// <summary>
    /// Challenge:
    /// Find students who have more than one active enrollment.
    /// Return the full name and the number of active courses.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, COUNT(*)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.IsActive = 1
    /// GROUP BY s.FirstName, s.LastName
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Challenge01_StudentsWithMoreThanOneActiveCourse()
    {
        var method = UniversityData.Students.Join(UniversityData.Enrollments, s=> s.Id, e=> e.StudentId,
            (s, e)=> new {s, e}).Where(e=> e.e.IsActive).GroupBy(e=> new {e.s.FirstName, e.s.LastName}).Where(e=> e.Count() > 1)
            .Select(e=> $"{e.Key.FirstName} {e.Key.LastName}, {e.Count()}");
        
        return method;
    }

    /// <summary>
    /// Challenge:
    /// List the courses that start in April 2026 and do not have any final grades assigned yet.
    ///
    /// SQL:
    /// SELECT c.Title
    /// FROM Courses c
    /// JOIN Enrollments e ON c.Id = e.CourseId
    /// WHERE MONTH(c.StartDate) = 4 AND YEAR(c.StartDate) = 2026
    /// GROUP BY c.Title
    /// HAVING SUM(CASE WHEN e.FinalGrade IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Challenge02_AprilCoursesWithoutFinalGrades()
    {
        var method = UniversityData.Courses.Where(c => (c.StartDate.Month == 4 && c.StartDate.Year == 2026))
            .Join(UniversityData.Enrollments, c => c.Id, e => e.CourseId, (c, e) => new { c, e })
            .GroupBy(e => e.c.Title).Where(e => e.All(e => e.e.FinalGrade == null))
            .Select(e => $"{e.Key}");

        return method;

    }

    /// <summary>
    /// Challenge:
    /// Calculate the average final grade for every lecturer across all of their courses.
    /// Ignore missing grades but still keep the lecturers in mind as the reporting dimension.
    ///
    /// SQL:
    /// SELECT l.FirstName, l.LastName, AVG(e.FinalGrade)
    /// FROM Lecturers l
    /// LEFT JOIN Courses c ON c.LecturerId = l.Id
    /// LEFT JOIN Enrollments e ON e.CourseId = c.Id
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY l.FirstName, l.LastName;
    /// </summary>
    public IEnumerable<string> Challenge03_LecturersAndAverageGradeAcrossTheirCourses()
    {
        var method = UniversityData.Lecturers.GroupJoin(UniversityData.Courses, l => l.Id, c=> c.LecturerId, 
            (l, c) => new {l, c}).SelectMany(lc => lc.c, (lc, c) => new {lc.l, c}).
            GroupJoin(UniversityData.Enrollments, lc=> lc.c.Id, e => e.CourseId, (lc, e) => new { lc, e })
            .SelectMany(lc => lc.e, (lc, e) => new {lc.lc, e}).Where(e=> e.e.FinalGrade != null).
            GroupBy(l=> new {l.lc.l.FirstName, l.lc.l.LastName}).
            Select(g => $"{g.Key.FirstName} {g.Key.LastName} - {g.Average(x => x.e.FinalGrade)}");
        return method;
    }

    /// <summary>
    /// Challenge:
    /// Show student cities and the number of active enrollments created by students from each city.
    /// Sort the result by the active enrollment count in descending order.
    ///
    /// SQL:
    /// SELECT s.City, COUNT(*)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.IsActive = 1
    /// GROUP BY s.City
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Challenge04_CitiesAndActiveEnrollmentCounts()
    {
        var method = UniversityData.Students.Join(UniversityData.Enrollments, s => s.Id, e => e.StudentId, (s, e) => new {s, e})
            .Where(e => e.e.IsActive).GroupBy(x=> x.s.City).OrderByDescending(e => e.Count()).Select(x => $"{x.Key}, {x.Count()}");
        
        return method;
    }

    private static NotImplementedException NotImplemented(string methodName)
    {
        return new NotImplementedException(
            $"Complete method {methodName} in Exercises/LinqExercises.cs and run the command again.");
    }
}
