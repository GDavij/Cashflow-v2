import { TestBed } from '@angular/core/testing';

import { FinancialDistributionService } from './financial-distribution.service';

describe('FinancialDistributionService', () => {
  let service: FinancialDistributionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FinancialDistributionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
