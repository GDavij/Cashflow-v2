import { monthFromIndex } from "../enums/MONTHS";

export class DateHelper {
  private static get currentDate(): Date {
    return new Date();
  }

  public static get currentYear(): number {
    return this.currentDate.getUTCFullYear();
  }

  public static get currentMonth(): number {
    return this.currentDate.getUTCMonth() + 1;
  }

  public static getMonthName(index: number): string {
    return monthFromIndex(index);
  }

  private year: number;
  private month: number;

  public constructor() {
    this.year = DateHelper.currentYear;
    this.month = DateHelper.currentMonth;
  }

  public goToNextYear() {
    this.month = 1;
    this.year += 1;
  }

  public goToPreviousYear() {
    this.month = 12;
    this.year -= 1;
  }

  public goToNextMonth() {
    if (this.month == 12) {
      this.goToNextYear();
      return;
    }

    this.month += 1;
  }

  public goToPreviousMonth() {
    if (this.month == 1) {
      this.goToPreviousYear();
      return;
    }

    this.month -= 1;
  }

  public getYear(): number {
    return this.year;
  }

  public getMonth(): number {
    return this.month;
  }

  public getMonthName(): string {
    return DateHelper.getMonthName(this.month);
  }

}
