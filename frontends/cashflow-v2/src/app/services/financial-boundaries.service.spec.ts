import { TestBed } from '@angular/core/testing';

import { FinancialBoundariesService } from './financial-boundaries.service';

describe('FinancialBoundariesService', () => {
  let service: FinancialBoundariesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FinancialBoundariesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
