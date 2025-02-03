import { Option } from "../components/select/select.models";
import { FINANCIAL_BOUNDARIES } from "../enums/FINANCIAL_BOUNDARIES";

export const FINANCIAL_BOUNDARIES_OPTIONS: Option<FINANCIAL_BOUNDARIES>[] = [
    {
        label: 'None',
        value: FINANCIAL_BOUNDARIES.NONE
    },
    {
        label: 'Money Limited',
        value: FINANCIAL_BOUNDARIES.MONEY
    },
    {
        label: 'Percentage over deposit by month',
        value: FINANCIAL_BOUNDARIES.PERCENTAGE_OVER_DEPOSIT
    }
]