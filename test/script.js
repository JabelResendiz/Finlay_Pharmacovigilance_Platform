import http from 'k6/http';

export const options = {
    vus: 1,
    duration: '2s'
};

export default function () {
    http.get(
        'http://localhost:5137/api/Report/get-report?notificationNumber=AEFI-20260610-L70CDYMN&token=jsdfhjkshfkj'
    );
}

export function handleSummary(data) {
    return {
        "summary.json": JSON.stringify(data.metrics, null, 2),
    };
}