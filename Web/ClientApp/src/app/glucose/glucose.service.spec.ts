import { TestBed, inject } from "@angular/core/testing";

import { GlucoseServiceService } from "./glucose.service";

describe('GlucoseServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GlucoseServiceService]
    });
  });

  it('should be created', inject([GlucoseServiceService], (service: GlucoseServiceService) => {
    expect(service).toBeTruthy();
  }));
});
