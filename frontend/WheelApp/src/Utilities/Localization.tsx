import { formatMessage } from 'umi';

export const L = (id: string, defaultMessage?: string) => {
  return formatMessage({id, defaultMessage});
}
