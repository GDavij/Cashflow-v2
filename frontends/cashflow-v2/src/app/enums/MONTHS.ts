export enum MONTHS {
  JANUARY = "January",
  FEBRUARY = "February",
  MARCH = 'March',
  APRIL = 'April',
  MAY = "May",
  JUNE = 'June',
  JULY = 'July',
  AUGUST = 'August',
  SEPTEMBER = 'September',
  OCTOBER = 'October',
  NOVEMBER = 'November',
  DECEMBER = 'December'
}


export function monthFromIndex(index: number): string {
  switch (index) {
    case 1: return MONTHS.JANUARY;

    case 2: return MONTHS.FEBRUARY;

    case 3: return MONTHS.MARCH;

    case 4: return MONTHS.APRIL;

    case 5: return MONTHS.MAY;

    case 6: return MONTHS.JUNE;

    case 7: return MONTHS.JULY;

    case 8: return MONTHS.AUGUST;

    case 9: return MONTHS.SEPTEMBER;

    case 10: return MONTHS.OCTOBER;

    case 11: return MONTHS.NOVEMBER;

    case 12: return MONTHS.DECEMBER;

    default: return "";
  }
}
