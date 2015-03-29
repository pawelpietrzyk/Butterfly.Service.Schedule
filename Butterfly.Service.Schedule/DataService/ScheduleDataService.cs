using Butterfly.Service.Schedule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Butterfly.Service.Schedule
{
    public class ScheduleDataService
    {
        public static ResultCodes DeleteDay(string dayId)
        {
            if (!String.IsNullOrEmpty(dayId))
            {
                using (DataEntities context = new DataEntities())
                {
                    DayTable dayTable = context.DayTable.FirstOrDefault(p => p.DayId == dayId);
                    if (dayTable != null)
                    {
                        context.DayTable.Remove(dayTable);
                        context.SaveChanges();
                        return ResultCodes.Deleted;
                    }
                    return ResultCodes.NotFound;
                }
            }
            return ResultCodes.EmptyParam;
        }
        public static ResultCodes AddUpdateDay(Day day)
        {
            if (day != null)
            {
                using (DataEntities context = new DataEntities())
                {
                    DayTable dayTable = context.DayTable.FirstOrDefault(p => p.DayId == day.DayId);
                    if (dayTable == null)
                    {
                        DayTable add = new DayTable() { DayId = day.DayId, DayOfWeek = day.DayOfWeek };
                        context.DayTable.Add(add);
                        context.SaveChanges();
                        return ResultCodes.Created;
                    }
                    else
                    {
                        dayTable.DayOfWeek = day.DayOfWeek;
                        context.SaveChanges();
                        return ResultCodes.Updated;
                    }
                }
            }
            return ResultCodes.NotCreated;
        }
        public static Day FindDay(DayOfWeek dayOfWeek)
        {
            int intDay = (int)dayOfWeek;
            using (DataEntities context = new DataEntities())
            {
                IQueryable<Day> days =
                    from day in context.DayTable
                    where (day.DayOfWeek == intDay)
                    select new Day() { DayId = day.DayId, DayOfWeek = day.DayOfWeek };
                return days.FirstOrDefault();
            }
        }
        public static SpecialDayTable FindSpecialDay(DateTime dt)
        {
            DateTime date = dt.Date;
            using (DataEntities context = new DataEntities())
            {
                IQueryable<SpecialDayTable> days =
                    from day in context.SpecialDayTable
                    where (day.Date == date)
                    select day;
                return days.FirstOrDefault();
            }
        }
        public static List<Day> GetDays(string _day)
        {
            if (!String.IsNullOrEmpty(_day))
            {
                using (DataEntities context = new DataEntities())
                {
                    IQueryable<Day> days =
                        from day in context.DayTable
                        where (day.DayId.Contains(_day))
                        select new Day() { DayId = day.DayId, DayOfWeek = day.DayOfWeek };
                    return days.ToList<Day>();
                }
            }
            else
            {
                using (DataEntities context = new DataEntities())
                {
                    IQueryable<Day> days =
                        from day in context.DayTable                        
                        select new Day() { DayId = day.DayId, DayOfWeek = day.DayOfWeek };
                    return days.ToList<Day>();
                }
            }            
        }
        public static List<DayGroup> GetDayGroups(string _dayGroup)
        {
            using (DataEntities context = new DataEntities())
            {
                if (String.IsNullOrEmpty(_dayGroup))
                {
                    IQueryable<DayGroup> dayGroups =
                        from dayGroup in context.DayGroupTable
                        select new DayGroup() { DayGroupId = dayGroup.DayGroupId, Description = dayGroup.Description };
                    return dayGroups.ToList<DayGroup>();
                }
                else
                {
                    IQueryable<DayGroup> dayGroups = 
                        from dayGroup in context.DayGroupTable
                        where (dayGroup.DayGroupId.Contains(_dayGroup))
                        select new DayGroup() { DayGroupId = dayGroup.DayGroupId, Description = dayGroup.Description };
                    return dayGroups.ToList<DayGroup>();
                }
            }
        }
        public static ResultCodes AddUpdateDayGroup(DayGroup dayGroup)
        {
            if (dayGroup != null)
            {
                using (DataEntities context = new DataEntities())
                {
                    DayGroupTable find = context.DayGroupTable.FirstOrDefault(p => p.DayGroupId == dayGroup.DayGroupId);
                    if (find != null)
                    {
                        find.Description = dayGroup.Description;
                        context.SaveChanges();
                        return ResultCodes.Updated;
                    }
                    else
                    {
                        DayGroupTable add = new DayGroupTable() { DayGroupId = dayGroup.DayGroupId, Description = dayGroup.Description };
                        context.DayGroupTable.Add(add);
                        context.SaveChanges();
                        return ResultCodes.Created;
                    }
                    
                }
            }
            return ResultCodes.NotCreated;
        }
        public static ResultCodes DeleteDayGroup(string dayGroupId)
        {
            if (!String.IsNullOrEmpty(dayGroupId))
            {
                using (DataEntities context = new DataEntities())
                {
                    DayGroupTable delete = context.DayGroupTable.FirstOrDefault(p => p.DayGroupId == dayGroupId);
                    if (delete != null)
                    {
                        context.DayGroupTable.Remove(delete);
                        context.SaveChanges();
                        return ResultCodes.Deleted;
                    }
                    return ResultCodes.NotFound;
                }
            }
            return ResultCodes.EmptyParam;
        }

        public static ResultCodes AddScheduleEntry(Entry entry)
        {
            if (entry != null)
            {                
                using (DataEntities context = new DataEntities())
                {
                    ScheduleEntry sch = new ScheduleEntry();
                    sch.ParamId = entry.ParamId;
                    sch.Hour = entry.Hour;
                    sch.Minute = entry.Minute;
                    sch.Time = (entry.Hour * 60) + entry.Minute;                    
                    context.ScheduleEntry.Add(sch);
                    context.SaveChanges();
                    return ResultCodes.Created;
                }
            }
            return ResultCodes.NotCreated;
        }
        public static List<ScheduleEntry> GetEntries(DateTime dateTime)
        {
            SpecialDayTable specialDay = ScheduleDataService.FindSpecialDay(dateTime);
            Day currentDay = ScheduleDataService.FindDay(dateTime.DayOfWeek);

            ScheduleParam param = null;
            using (DataEntities context = new DataEntities())
            {
                IQueryable<ScheduleParam> schParams = 
                from p in context.ScheduleParam
                join specrel in context.SpecialDayGroupDayRelation on p.SpecialDateGroupId equals specrel.SpecialDayGroupId
                join dayrel in context.DayGroupRelation on p.DayGroupId equals dayrel.DayGroupId
                where (currentDay != null ? dayrel.DayId == currentDay.DayId : true)
                || (specialDay != null ? specrel.SpecialDayId == specialDay.SpecialDayId : true)
                select p;

                param = schParams.FirstOrDefault();
            }
            
            
            if (currentDay != null)
            {
                int time = (dateTime.Hour * 60) + dateTime.Minute;
                using (DataEntities context = new DataEntities())
                {
                    IQueryable<ScheduleEntry> entry =
                    from dayRel in context.DayGroupRelation
                    join schParam in context.ScheduleParam on dayRel.DayGroupId equals schParam.DayGroupId
                    join schEntry in context.ScheduleEntry on schParam.ParamId equals schEntry.ParamId
                    where (dayRel.DayId == currentDay.DayId && schEntry.Time > time)
                    select schEntry;
                    return entry.ToList();
                }
            }
            return new List<ScheduleEntry>();
        }
    }
}
